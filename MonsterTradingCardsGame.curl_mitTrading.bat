@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"Admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo. 
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"Admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

REM --------------------------------------------------
echo 3) acquire packages kienboec
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
echo should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
echo.

REM --------------------------------------------------
echo 4) acquire packages altenhof
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo should fail (no money):
curl -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo.


REM --------------------------------------------------
echo 5) show all acquired cards kienboec
curl -X GET http://localhost:10001/cards --header "Authorization: Bearer kienboec-mtcgToken"
echo should fail (no token)
curl -X GET http://localhost:10001/cards 
echo.
echo.

REM --------------------------------------------------
echo 6) show all acquired cards altenhof
curl -X GET http://localhost:10001/cards --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 7) show unconfigured deck
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test20-mtcgToken"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test17-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 8) configure deck
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer test20-mtcgToken" -d "[\"60\", \"65\", \"67\", \"68\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test20-mtcgToken"
echo.
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer test17-mtcgToken" -d "[\"90\", \"91\", \"92\", \"93\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test17-mtcgToken"
echo.
echo.
echo should fail and show original from before:
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer test17-mtcgToken" -d "[\"60\", \"65\", \"67\", \"68\"]"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test17-mtcgToken"
echo.
echo.
echo should fail ... only 3 cards set
curl -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer test20-mtcgToken" -d "[\"60\", \"65\", \"67\"]"
echo.


REM --------------------------------------------------
echo 9) show configured deck 
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test20-mtcgToken"
echo.
curl -X GET http://localhost:10001/deck --header "Authorization: Bearer test17-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 10) show configured deck different representation
echo kienboec
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer test20-mtcgToken"
echo.
echo.
echo altenhof
curl -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer test17-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 11) edit user data
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Username\": \"kienboec\",  \"Password\": \"daniel\", \"NewPassword\": \"danielFisher\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Username\": \"altenhof\", \"Password\": \"markus\",  \"NewPassword\": \"codingFan\"}"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.
echo should fail:
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer altenhof-mtcgToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Username\": \"kienboec\",  \"Password\": \"markus\",  \"NewPassword\": \"codingFan\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Username\": \"kienboec\",  \"Password\": \"different\", \"NewPassword\": \"danielFisher\"}"
echo.
curl -X GET http://localhost:10001/users/someGuy  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 12) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 13) scoreboard
curl -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 14) battle
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Bearer kienboec-mtcgToken"
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken"
ping localhost -n 10 >NUL 2>NUL

REM --------------------------------------------------
echo 15) Stats 
echo kienboec
curl -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo altenhof
curl -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 16) scoreboard
curl -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 17) trade
echo check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo create trading deal
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 15}"
echo.
echo check trading deals
curl -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo delete trading deals
curl -X DELETE http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

REM --------------------------------------------------
echo 18) check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 15}"
echo check trading deals
curl -X GET http://localhost:10001/tradings  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings  --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo try to trade with yourself (should fail)
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\""
echo.
echo try to trade 
echo.
curl -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "\"951e886a-0fbf-425d-8df5-af2ee4830d85\""
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -X GET http://localhost:10001/tradings --header "Authorization: Bearer altenhof-mtcgToken"
echo.

REM --------------------------------------------------
echo 19) Chatroom
REM Chatroom
echo Receive Chatroom Messages
curl -X GET http://localhost:10001/chatroom --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo Send Chatroom Message
curl -X POST http://localhost:10001/chatroom --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"MessageText\":\"Nachricht geschrieben\"}"
echo.
echo Receive Chatroom Messages
curl -X GET http://localhost:10001/chatroom --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.


REM --------------------------------------------------
echo end...

REM this is approx a sleep 
ping localhost -n 100 >NUL 2>NUL
@echo on
