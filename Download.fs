namespace FunctionalPatterns

module Download =

    type [<Measure>] ms

    type WebClientWithTimeout(timeout:int<ms>) = 
        inherit System.Net.WebClient()

        override this.GetWebRequest(address) =
            let result = base.GetWebRequest(address)
            result.Timeout <- int timeout
            result

    type UriContent =
        UriContent of System.Uri * string

    type UriContentSize = 
        UriContentSize of System.Uri * int

    let getUriContent (uri:System.Uri) = 
        async {
            use client = new WebClientWithTimeout(5000<ms>)
            try
                printfn " [%s] started ..." uri.AbsoluteUri
                let! html = client.AsyncDownloadString(uri)

                printfn " [%s] finished ..." uri.AbsoluteUri
                let uriContent = UriContent (uri, html)

                return (Result.Success uriContent)
            with
            | ex ->
                printfn " [%s] failed ..." uri.AbsoluteUri
                let err = sprintf "[%s] %A" uri.AbsoluteUri ex.Message
                return Result.Failure [err]
        }

    let makeContentSize (UriContent (uri, html)) =
        if System.String.IsNullOrEmpty(html) then
            Result.Failure ["empty page"]
        else
            let uriContentSize = UriContentSize (uri, html.Length)
            Result.Success uriContentSize

    let getUriContentSize uri =
        getUriContent uri 
        |> Async.map (Result.bind makeContentSize)

    let maxContentSize list =
        let contentSize (UriContentSize (_, len)) = len
        list |> List.maxBy contentSize

    let largestPageSizeAsyncResultM uris = 
        uris
        |> List.map (System.Uri >> getUriContentSize)
        |> List.sequenceAsyncResultM
        |> AsyncResult.map maxContentSize
        
    let largestPageSizeAsyncResultA uris = 
        uris
        |> List.map (System.Uri >> getUriContentSize)
        |> List.sequenceAsyncResultA
        |> AsyncResult.map maxContentSize
        
    let largestPageSizeM1 uris = 
        uris
        |> List.map System.Uri
        |> List.map getUriContentSize
        |> List.sequenceAsyncM
        |> Async.map List.sequenceResultM
        |> Async.map (Result.map maxContentSize)

    let largestPageSizeA1 uris = 
        uris
        |> List.map System.Uri
        |> List.map getUriContentSize
        |> List.sequenceAsyncA
        |> Async.map List.sequenceResultA
        |> Async.map (Result.map maxContentSize)

    let largestPageSizeA2 uris = 
        uris
        |> List.traverseAsyncA (System.Uri >> getUriContentSize)
        |> Async.map (List.sequenceResultA >> Result.map maxContentSize)

    let showContentSizeResult result =
        match result with
        | Result.Success (UriContentSize (uri, len)) -> 
            printfn "SUCCESS: [%s] Content size is %i" uri.Host len
        | Result.Failure errs ->
            printfn "FAILURE: %A" errs
         
    let showContentResult result =
        match result with
        | Result.Success (UriContent (uri, html)) ->
            printfn "SUCCESS: [%s] First 100 chars: %s" uri.Host (html.Substring(0,100))
        | Result.Failure errs ->
            printfn "Failure: %A" errs

    let time countN label f =
        let stopWatch = System.Diagnostics.Stopwatch()

        System.GC.Collect()
        printfn "===================="
        printfn "%s" label
        printfn "===================="

        let mutable totalMs = 0L
        for iteration in [1..countN] do
            stopWatch.Restart()
            f()
            stopWatch.Stop()
            printfn "#%2i elapsed:%6ims " iteration stopWatch.ElapsedMilliseconds
            totalMs <- totalMs + stopWatch.ElapsedMilliseconds

        let avgTimePerRun = totalMs / int64 countN
        printfn "%s: Average time per run: %6ims " label avgTimePerRun
