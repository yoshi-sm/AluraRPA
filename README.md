<a name="top"></a>
# Alura RPA
RPA de raspagem de tela no site Alura



## Table of Contents
- [About](#about)
- [Setup e Instalação](#setup-e-instalação)
- [Utilização](#utilização)
- [Estrutura de Código](#estrutura-de-código)
- [Dependências](#dependências)

## About

Alura RPA é um RPA leve baseado em Selenium e C# que realiza buscas por cursos no site https://www.alura.com.br/ utilizando o navegador Chrome e retorna dados dos cursos encontrados em um banco de dados local SQLite
São esses dados:

- Título do curso
- Nome do Professor
- Carga Horária
- Descrição

Além disso as pesquisas são discriminadas pelas palavras-chave utilizadas para realizar a pesquisa e são checadas para que não haja duplicações no banco local.

## Setup e Instalação

### Pré-Requisitos

A solução utiliza .Net 8 e foi escrita utilizando a versão 131.0.6778.70 do Chrome, versões diferentes podem apresentar problemas.

### Instalação

Para instalar a aplicação basta executar os seguintes comandos no terminal na pasta desejada:

- git clone https://github.com/yoshi-sm/AluraRPA.git
- cd AluraRPA
- dotnet build

A partir deste ponto, para executar a aplicação basta executar o seguinte comando na pasta "AluraRPA" onde se encontra a solução:

- dotnet run


## Utilização

Por padrão, a aplicação irá realizar uma buscar pelo termo "string", este termo pode ser alterado, fazendo uma alteração no segundo argumento no método "ExecutarRPA" 
no arquivo "Program.cs":

![image](https://github.com/user-attachments/assets/b1ff5466-3d91-43c7-b230-ef30cf7bf81a)

Caso seja a primeira vez que a aplicação esteja sendo executada, será criada uma instância do banco SQLite em C:\Users\Usuário\AppData\Local\Temp\AluraDb\.
O local onde on banco de dados é criado e mantido pode ser alterado na classe BancoLocal, na pasta de Infra.

Ao fim da execução os dados são salvos e além disso é salvo também o termo utilizado para a pesquisa. Este termo é utilizado para realizar um filtro
e evitar duplicações no banco de dados.

## Estrutura de Código 

A aplicação segue uma estrutura de código simples, seguindo um modelo "DDD" onde possível.

    .
    ├── application               # Camada de aplicação
    |   ├── interfaces            # Interface utilizada pela camada de aplicação
    |   ├── webDriver             # Pasta do WebDriver
    ├── domain                    # Camada de Domínio
    |   ├── entity                # Pasta das entidades
    ├── infra                     # Camada de Infra
    ├── PROGRAM.cs                # Arquivo que executa a aplicação
    └── README.md

## Dependências

Dapper==2.1.35<br/> 
Microsoft.Data.Sqlite==9.0.0<br/> 
Microsoft.Extensions.DependencyInjection==9.0.0<br/> 
Selenium.Support==4.26.1<br/> 
Selenium.WebDriver==4.26.1<br/> 
Selenium.WebDriver.ChromeDriver==130.0.6723.11600<br/> 


[Back to top](#top)
