@echo off
echo Iniciando banco de dados via Docker...
docker-compose up -d

echo Compilando e executando a API...
cd API
dotnet build
start cmd /k "dotnet run --urls=http://localhost:5042"

echo Compilando e executando o MVC...
cd ../MVC
dotnet build
start cmd /k "dotnet run --urls=http://localhost:5292"

echo Todos os serviços foram iniciados. Acesse as URLs abaixo:
echo API: http://localhost:5042
echo MVC: http://localhost:5292
pause
