namespace FunctionalPatterns

open Products
open ApiAction

module main =

    let showResult result =
        match result with
        | Result.Success (productInfoList) ->
            printfn "SUCCESS: %A" productInfoList
        | Result.Failure errs ->
            printfn "FAILURE: %A" errs

    let setupData (apiClient:ApiClient) = 
        apiClient.Set(CustomerId "C1") [ProductId "P1"; ProductId "P2"] |> ignore
        apiClient.Set(CustomerId "C2") [ProductId "P1"; ProductId "PX"] |> ignore

        apiClient.Set(ProductId "P1") {Name="P1.Name"} |> ignore
        apiClient.Set(ProductId "P2") {Name="P2.Name"} |> ignore

    let setupAction = ApiAction setupData

    [<EntryPoint>]
    let main argv =        
        execute setupAction

        CustomerId "C1"
        |> getPurchaseInfo
        |> ApiAction.execute
        |> showResult

        CustomerId "CX"
        |> getPurchaseInfo
        |> ApiAction.execute
        |> showResult

        CustomerId "C2"
        |> getPurchaseInfo
        |> ApiAction.execute
        |> showResult
        0
