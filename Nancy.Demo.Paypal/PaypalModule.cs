namespace Nancy.Demo.Paypal
{
    using PayPal.Api;
    using System;
    using System.Collections.Generic;

    public class PaypalModule : NancyModule
    {
        public PaypalModule()
        {
            Get["/paypalPayment"] = _ =>
            {
                var apiContext = GetAPIContext();

                // Make an API call
                var payment = Payment.Create(apiContext, new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "Transaction description.",
                            invoice_number = Guid.NewGuid().ToString(),
                            amount = new Amount
                            {
                                currency = "USD",
                                total = "100.00",
                                details = new Details
                                {
                                    tax = "15",
                                    shipping = "10",
                                    subtotal = "75"
                                }
                            },
                            item_list = new ItemList
                            {
                                items = new List<Item>
                                {
                                    new Item
                                    {
                                        name = "Item Name",
                                        currency = "USD",
                                        price = "15",
                                        quantity = "5",
                                        sku = "sku"
                                    }
                                }
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = "http://localhost:3579/return",
                        cancel_url = "http://localhost:3579/cancel"
                    }
                });

                return Response.AsRedirect(payment.GetApprovalUrl(), 
                    Responses.RedirectResponse.RedirectType.Permanent);
            };

            Get["/return"] = _ =>
            {
                var a = Request.Headers;

                var apiContext = GetAPIContext();

                Payment paymentResult = Payment.Execute(apiContext, Request.Query["paymentId"], 
                    new PaymentExecution { payer_id = Request.Query["PayerID"] });
                
                if (paymentResult.state == "approved")
                {
                    return "Your payment was successful!";
                } else
                {
                    return "Sorry, your payment failed.";
                } 
            };

            Get["cardPayment"] = _ =>
            {
                var apiContext = GetAPIContext();

                // Make an API call
                var payment = Payment.Create(apiContext, new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "credit_card",
                        funding_instruments = new List<FundingInstrument> {
                            new FundingInstrument
                            {
                                credit_card = new CreditCard
                                {
                                    billing_address = new Address {
                                        city = "New York",
                                        country_code = "US",
                                        line1 = "20 W 29th St",
                                        postal_code = "10001",
                                        state = "NY"
                                    },
                                    cvv2 = "854",
                                    expire_month = 10,
                                    expire_year = 2020,
                                    first_name = "John", 
                                    last_name = "Scott",
                                    number = "xxxxxxxxxxxxxxxx",
                                    type = "visa"
                                }
                            }
                        }
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = "Transaction description.",
                            invoice_number = Guid.NewGuid().ToString(),
                            amount = new Amount
                            {
                                currency = "USD",
                                total = "100.00",
                                details = new Details
                                {
                                    tax = "15",
                                    shipping = "10",
                                    subtotal = "75"
                                }
                            },
                            item_list = new ItemList
                            {
                                items = new List<Item>
                                {
                                    new Item
                                    {
                                        name = "Item Name",
                                        currency = "USD",
                                        price = "15",
                                        quantity = "5",
                                        sku = "sku"
                                    }
                                }
                            }
                        }
                    }
                });

                if (payment.state == "approved")
                {
                    return "Your payment was processed and your credit card will be charged soon!";
                }
                else
                {
                    return "Sorry, your payment failed.";
                }
            };
        }

        // Authenticate with PayPal
        public static APIContext GetAPIContext()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken);
        }
    }
}
