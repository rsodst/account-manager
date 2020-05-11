## Prerequirements
### Before start using sample application, configure **PostgresConnectionOptions**
### Start App.Api project and optionaly start MessageHandler. In this moment handler added as example of service bus intergration
___
## Main scenario
1. Getting bearer token
  * Use POST **SignUp** for create new user __{host}/user/signup__
  * Use POST **SignIn** for auth with exist user __{host}/user/signin__
2. Use POST **profile-confirmation** method for getting profileConfirmation __{host}/debug/profile-confirmation__
3. Use POST **accounts/create** for create new account __{host}/accounts/create__
4. Use POST **accounts/refil** for create new account __{host}/accounts/refill__
5. Use POST **accounts/transfer** for transfer money with source to destination __{host}/accounts/transfer__
  * Currencies in operated accounts must be equivalent, for complete transfer transaction
