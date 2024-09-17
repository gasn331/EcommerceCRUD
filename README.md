# E-commerce API & MVC Application

 Este projeto consiste em uma aplicação de e-commerce com back-end em .NET 8.0. 
*O projeto contém uma API para gerenciamento de produtos e uma aplicação MVC para visualização e interação.
* O banco de dados utilizado é o MySQL, que será executado via Docker, as tabelas são de Produtos, Departamentos, Autenticacao e ProdutoAudit.
* A Autenticação é feita utilizando um mock, de forma que as senhas armazenadas são valores base64.

## Requisitos

Certifique-se de que os seguintes softwares estão instalados no seu sistema:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

## Passos para Configuração

### 1. Clonando o Repositório

Execute o seguinte comando para clonar o repositório:

```bash
git clone https://github.com/gasn331/EcommerceMaximaTech.git
cd seu-repositorio
```` 

### 2. Subindo o Banco de Dados com Docker

O banco de dados MySQL será executado via Docker. Certifique-se de que o Docker está instalado e em execução.

Execute o comando a seguir para iniciar o contêiner do MySQL:

```bash
docker-compose up -d
```

Este comando irá:

Baixar a imagem do MySQL, caso ainda não esteja no seu sistema.
Criar um contêiner MySQL com as seguintes configurações:
	Porta: 3306
	Database: ecommercedb
	Usuário: admin
	Senha: admin


### 3. Compilando e Executando a API

A API roda na porta 5042. Para compilar e rodar a API, siga os passos abaixo:

```bash
cd API
dotnet build
dotnet run --urls=http://localhost:5042
```


### 4. Compilando e Executando o MVC

A aplicação MVC roda na porta 5292. Para compilar e executar o projeto MVC, siga os passos abaixo:

```bash
cd ../MVC
dotnet build
dotnet run --urls=http://localhost:5292
```

### 6. URLs

- **API**: `http://localhost:5042`
- **MVC**: `http://localhost:5292`

# 7. Verificando o Banco de Dados

Caso queira verificar o banco de dados via linha de comando, você pode utilizar o comando a seguir para entrar no contêiner MySQL:

```bash
docker exec -it <container-id> mysql -uadmin -padmin
```

Substitua <container-id> pelo ID do contêiner MySQL que pode ser obtido com:

```bash
docker ps
```


### 8. Execução automatizada 

Basta rodar o arquivo para iniciar tanto o banco de dados via Docker quanto a API e o MVC:

```bash
start run_project.bat
```

Este script abrirá duas janelas de comando, uma para a API e outra para a aplicação MVC, e deixará ambos os serviços rodando em segundo plano.