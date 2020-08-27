hosting was used:
https://member5-5.smarterasp.net/cp/cp_screen
login: TransfersSPBApp3
password: TransfersSPBApp3

if create new account
setup database:

1. update connectionString
from Database tab - use connection string for ASP.NET 
example: "Data Source=SQL5063.site4now.net;Initial Catalog=DB_A64D3F_TransfersSPBApp3;User Id=DB_A64D3F_TransfersSPBApp3_admin;Password=YOUR_DB_PASSWORD
and place into Web.config

2. run update-database in powershell console

database should be ready and empty


update web deploy config
project -> publish -> edit profile
use data from screenshot attached webDeployInfo