namespace FunctionalPatterns

open Reader

module Products =

    type CustomerId = CustomerId of string
    type ProductId = ProductId of string
    type ProductInfo = {Name:string}

    let getPurchaseIds (customerId:CustomerId) =
        let action (environment:Environment) = 
            environment.Get<ProductId list> customerId
        Reader.Reader action

    let getProductInfo (productId:ProductId) =
        let action (environment:Environment) =
            environment.Get<ProductInfo> productId
        Reader action

    let getPurchaseInfo =
        let getProduct = List.traverseReaderResultM getProductInfo
        getPurchaseIds >> ReaderResult.bind getProduct
