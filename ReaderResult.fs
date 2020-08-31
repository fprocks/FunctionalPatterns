namespace FunctionalPatterns

module ReaderResult = 

    let map f =
        Reader.map (Result.map f)

    let retn x =
        Reader.retn (Result.retn x)

    let apply fReaderResult xReaderResult =
        let newAction environment =
            let fResult = Reader.run environment fReaderResult
            let xResult = Reader.run environment xReaderResult
            Result.apply fResult xResult
        Reader.Reader newAction

    let bind f xActionResult = 
        let newAction environment = 
            let xResult = Reader.run environment xActionResult
            let yAction = 
                match xResult with
                | Result.Success x ->
                    f x
                | Result.Failure errs ->
                    (Result.Failure errs) |> Reader.retn
            Reader.run environment yAction
        Reader.Reader newAction


