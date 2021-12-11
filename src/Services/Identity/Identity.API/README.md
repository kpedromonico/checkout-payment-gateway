# Important
There is no email authentication due to not having a proper SMTP server enabled. So whenever you are registering a new user, you will receive a JWT straight away.

## Database
For simplicity / lower resource consumption purposes, I have decided to use an InMemory Database.

## JWT Tokens
For this project, I went with the "Refresh Token Rotation" when it comes to the JWT validation. Meaning that everytime the application exchanges a JWT along the refresh token, both will be refreshed an replied back to the user. [More details here](https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/#Refresh-Token-Rotation)