Nancy.Demo.Paypal
===

A demo for a simple Nancy + Paypal SDK integration.

Requirements
---

- Visual Studio 2015
- .NET Framework 4.6

Install instructions
---

- Clone this repository;
- Set your own client ID and Secret (generated from creating a new app [here](https://developer.paypal.com));
- Add a credit card number in the '/cardPayment' route (add a bogus one just for testing purposes);
- Run the project;

Usage
---
| Method           | Description
| -----------------|:--------------------------------------------:|
| `/paypalPayment` | Process a payment using Paypal.              |
| `/cardPayment`   | Process a payment using a credit card.       |
| `/return`        | Callback URL for the `/paypalPayment` route. |

Missing features
---

- Capture payments for later;
- Refund payments;
- Billing plans;
- [And more](https://developer.paypal.com/docs).