namespace FunctionalPatterns

//open Products

//module main =

//    let showResult result =
//        match result with
//        | Result.Success (productInfoList) ->
//            printfn "SUCCESS: %A" productInfoList
//        | Result.Failure errs ->
//            printfn "FAILURE: %A" errs

//    let setupTestData (api:ApiAction.ApiClient) = 
//        api.Set(CustomerId "C1") [ProductId "P1"; ProductId "P2"] |> ignore
//        api.Set(CustomerId "C2") [ProductId "PX"; ProductId "P2"] |> ignore

//        api.Set(ProductId "P1") {Name="P1.Name"} |> ignore
//        api.Set(ProductId "P2") {Name="P2.Name"} |> ignore

//    let setupAction = ApiAction.ApiAction setupTestData

//    [<EntryPoint>]
//    let main argv =        
//        ApiAction.execute setupAction

//        CustomerId "C1"
//        |> getPurchaseInfo
//        |> ApiAction.execute
//        |> showResult

//        //CustomerId "CX"
//        //|> getPurchaseInfo
//        //|> ApiAction.execute
//        //|> showResult

//        //CustomerId "C2"
//        //|> getPurchaseInfo
//        //|> ApiAction.execute
//        //|> showResult
//        0
