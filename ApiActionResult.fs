namespace FunctionalPatterns
 
module ApiActionResult = 

    let map f =
        ApiAction.map (Result.map f)

    let retn x =
        ApiAction.retn (Result.retn x)

    let apply fActionResult xActionResult =
        let newAction api =
            let fResult = ApiAction.run api fActionResult
            let xResult = ApiAction.run api xActionResult
            Result.apply fResult xResult
        ApiAction.ApiAction newAction

    let bind f xActionResult = 
        let newAction api = 
            let xResult = ApiAction.run api xActionResult
            let yAction = 
                match xResult with
                | Result.Success x ->
                    f x
                | Result.Failure errs ->
                    (Result.Failure errs) |> ApiAction.retn
            ApiAction.run api yAction
        ApiAction.ApiAction newAction


