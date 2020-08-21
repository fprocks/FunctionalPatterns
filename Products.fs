namespace FunctionalPatterns

open ApiAction

module Products =

    type CustomerId = CustomerId of string
    type ProductId = ProductId of string
    type ProductInfo = {Name:string}

    let getPurchaseIds (customerId:CustomerId) =
        let action (api:ApiClient) = 
            api.Get<ProductId list> customerId
        ApiAction.ApiAction action

    let getProductInfo (productId:ProductId) =
        let action (api:ApiClient) =
            api.Get<ProductInfo> productId
        ApiAction.ApiAction action

    let getPurchaseInfo = 
        let getProductInfoLifted =  
            getProductInfo
            |> List.traverseApiActionResultM
            |> ApiActionResult.bind
        getPurchaseIds >> getProductInfoLifted
