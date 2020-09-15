namespace FunctionalPatterns
//
//open Products
//
//module main =
//
//    let showResult result =
//        match result with
//        | Result.Success (productInfoList) ->
//            printfn "SUCCESS: %A" productInfoList
//        | Result.Failure errs ->
//            printfn "FAILURE: %A" errs
//
//    let setupData (environment:Reader.Environment) = 
//        environment.Set(CustomerId "C1") [ProductId "P1"; ProductId "P2"] |> ignore
//        environment.Set(CustomerId "C2") [ProductId "P1"; ProductId "PX"] |> ignore
//
//        environment.Set(ProductId "P1") {Name="P1.Name"} |> ignore
//        environment.Set(ProductId "P2") {Name="P2.Name"} |> ignore
//
//    let setupAction = Reader.Reader setupData
//
//    [<EntryPoint>]
//    let main argv =        
//        Reader.execute setupAction
//
//        CustomerId "C1"
//        |> getPurchaseInfo
//        |> Reader.execute
//        |> showResult
//
//        CustomerId "CX"
//        |> getPurchaseInfo
//        |> Reader.execute
//        |> showResult
//
//        CustomerId "C2"
//        |> getPurchaseInfo
//        |> Reader.execute
//        |> showResult
//        0
