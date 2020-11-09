namespace FunctionalPatterns

open ApiAction

module Products =

    type CustomerId = CustomerId of string
    type ProductId = ProductId of string
    type ProductInfo = {Name:string}

    let getPurchaseIds (customerId:CustomerId) =
        let action (apiClient:ApiClient) = 
            apiClient.Get<ProductId list> customerId
            
        ApiAction action

    let getProductInfo (productId:ProductId) =
        let action (apiClient:ApiClient) =
            apiClient.Get<ProductInfo> productId
            
        ApiAction action

    let getPurchaseInfo =
        let getProduct = List.traverseReaderResultM getProductInfo
        getPurchaseIds >> ApiActionResult.bind getProduct
