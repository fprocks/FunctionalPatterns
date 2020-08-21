namespace FunctionalPatterns

module AsyncResult =
    
    let map f =
        f |> Result.map |> Async.map

    let retn x =
        x |> Result.retn |> Async.retn

    let apply fAsyncResult xAsyncResult =
        fAsyncResult |> Async.bind (fun fResult ->
        xAsyncResult |> Async.map (fun xResult ->
        Result.apply fResult xResult))

    let bind f xAsyncResult = async {
        let! xResult = xAsyncResult
        match xResult with
        | Result.Success x -> return! f x
        | Result.Failure err -> return (Result.Failure err)
        }