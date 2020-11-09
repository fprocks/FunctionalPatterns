namespace FunctionalPatterns

module List =
    let traverseAsyncA f list =
        let (<*>) = Async.apply
        let retn = Async.retn

        let cons head tail = head :: tail

        let initState = retn []
        let folder head tail = 
            retn cons <*> (f head) <*> tail

        List.foldBack folder list initState

    let traverseAsyncM f list = 
        let (>>=) x f = Async.bind f x
        let retn = Async.retn
        
        let cons head tail = head :: tail

        let initState = retn []

        let folder head tail = 
            f head >>= (fun h ->
            tail >>= (fun t ->
            retn (cons h t)))

        List.foldBack folder list initState

    let sequenceAsyncA x = traverseAsyncA id x
    let sequenceAsyncM x = traverseAsyncM id x

    let traverseResultA f list = 
        let (<*>) = Result.apply
        let retn = Result.Success

        let cons head tail = head :: tail

        let initState = retn []
        let folder head tail = retn cons <*> (f head) <*> tail

        List.foldBack folder list initState

    let traverseResultM f list = 
        let (>>=) x f = Result.bind f x
        let retn = Result.retn

        let cons head tail = head :: tail
        
        let initState = retn []
        let folder head tail = 
            f head >>= (fun h -> 
            tail >>= (fun t ->
            retn (cons h t)))

        List.foldBack folder list initState

    let sequenceResultA x = traverseResultA id x
    let sequenceResultM x = traverseResultM id x

    let traverseAsyncResultM f list = 
        let (>>=) x f = AsyncResult.bind f x
        let retn = AsyncResult.retn

        let cons head tail = head :: tail

        let initState = retn []
        let folder head tail =
            f head >>= (fun h ->
            tail >>= (fun t ->
            retn (cons h t)))

        List.foldBack folder list initState
        
    let traverseAsyncResultA f list =
        let (<*>) = AsyncResult.apply
        let retn = AsyncResult.retn

        let cons head tail = head :: tail
        let initState = retn []

        let folder head tail = 
            retn cons <*> f head <*> tail

        List.foldBack folder list initState
        
    let sequenceAsyncResultM x = traverseAsyncResultM id x
    let sequenceAsyncResultA x = traverseAsyncResultA id x
   
    let traverseApiActionResultA f list =
        let (<*>) = ApiActionResult.apply
        let retn = ApiActionResult.retn

        let cons head tail = head :: tail
        let initState = retn []

        let folder head tail = 
            retn cons <*> f head <*> tail

        List.foldBack folder list initState

    let traverseApiActionResultM f list =
        let (>>=) x f = ApiActionResult.bind f x
        let retn = ApiActionResult.retn

        let cons head tail = head :: tail

        let initState = retn []
        let folder head tail =
            f head >>= (fun h ->
            tail >>=(fun t ->
            retn (cons h t)))

        List.foldBack folder list initState