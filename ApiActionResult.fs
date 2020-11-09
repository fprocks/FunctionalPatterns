namespace FunctionalPatterns

module ApiActionResult = 

    let map f =
        ApiAction.map (Result.map f)

    let retn x =
        ApiAction.retn (Result.retn x)

    let apply fApiActionResult xApiActionResult =
        let newAction apiClient =
            let fResult = ApiAction.run apiClient fApiActionResult
            let xResult = ApiAction.run apiClient xApiActionResult
            Result.apply fResult xResult
            
        ApiAction.ApiAction newAction

    let bind f xActionResult = 
        let newAction apiClient = 
            let xResult = ApiAction.run apiClient xActionResult
            let yAction = 
                match xResult with
                | Result.Success x ->
                    f x
                | Result.Failure errs ->
                    (Result.Failure errs) |> ApiAction.retn
            ApiAction.run apiClient yAction
            
        ApiAction.ApiAction newAction


