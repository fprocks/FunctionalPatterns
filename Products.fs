namespace FunctionalPatterns

open ApiAction

module Products =

    type CustomerId = CustomerId of string
    type ProductId = ProductId of string
    type Product = {Name:string}

    let getProductIds (customerId:CustomerId) =
        let action (apiClient:ApiClient) = 
            apiClient.Get<ProductId list> customerId
            
        ApiAction action

    let getProduct (productId:ProductId) =
        let action (apiClient:ApiClient) =
            apiClient.Get<Product> productId
            
        ApiAction action

    let getProducts =
        let getProducts = List.traverseApiActionResultM getProduct
        getProductIds >> ApiActionResult.bind getProducts