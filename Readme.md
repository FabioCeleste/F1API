# 🌐 **F1 Rest API**

[![.NET 7](https://img.shields.io/badge/.NET-7.0-blueviolet?logo=.net)](https://dotnet.microsoft.com/download/dotnet/7.0)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14-blue?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-green?logo=swagger)](https://swagger.io/tools/swagger-ui/)
[![Docker](https://img.shields.io/badge/Docker-19.03.13-blue?logo=docker&logoColor=white)](https://www.docker.com/)
[![GitHub Actions](https://img.shields.io/badge/CI-GitHub_Actions-blue?logo=githubactions&logoColor=white)](https://github.com/features/actions)
[![Prometheus](https://img.shields.io/badge/Monitoring-Prometheus-orange?logo=prometheus&logoColor=white)](https://prometheus.io/)
[![Grafana](https://img.shields.io/badge/Visualization-Grafana-orange?logo=grafana&logoColor=white)](https://grafana.com/)

## 📚 **Descrição**

A **F1 Rest API** é um projeto construído em .NET 7 que fornece uma API REST modelada no estilo da PokeAPI, mas para dados da Fórmula 1. Os dados utilizados são extraídos de arquivos CSV e importados para o banco de dados utilizando scripts em JavaScript. Este projeto oferece uma API para gerenciar informações sobre pilotos e construtores da Fórmula 1. A documentação da API é interativa e acessível através do Swagger.

Os dados são provenientes do [dataset da Fórmula 1 World Championship 1950-2020 disponível no Kaggle](https://www.kaggle.com/datasets/rohanrao/formula-1-world-championship-1950-2020/discussion?sort=hotness).

## 🚀 **Funcionalidades**

-   📋 CRUD de Pilotos e Construtores
-   🔗 Relacionamentos muitos-para-muitos entre Pilotos e Construtores
-   🧪 Testes Unitários para garantir a qualidade do código
-   📈 Monitoramento com Prometheus e visualização com Grafana
-   🔒 Autenticação JWT
-   🛠️ Documentação com Swagger

## 🛠 **Tecnologias Utilizadas**

-   **Linguagem**: C# com .NET 7
-   **Banco de Dados**: PostgreSQL
-   **Documentação**: Swagger
-   **Monitoramento**: Prometheus
-   **Visualização**: Grafana
-   **Containerização**: Docker

## 🚀 **Deploy**

A **F1 Rest API** está implantada em um Orange Pi Zero 3, utilizando containers Docker para gerenciar a aplicação e os serviços associados. O ambiente de produção foi configurado para garantir a estabilidade e a eficiência do sistema, aproveitando a leveza e a flexibilidade dos containers Docker.

### 🖥 **Especificações do Sistema**

-   **Processador**: Quad-core Cortex-A55
-   **Memória**: 2 GB DDR3
-   **Armazenamento**: Armazenamento via cartão SD
-   **Sistema Operacional**: Alpine Linux
-   **Containers**: Containers Docker para a API, PostgreSQL, Prometheus e Grafana

Este ambiente foi escolhido para otimizar o consumo de energia e o custo, mantendo a capacidade necessária para rodar todos os componentes do projeto.

## 📦 **Instalação**

1. Clone o repositório:

    ```sh
    git clone https://github.com/FabioCeleste/F1API.git
    ```

2. Navegue até o diretório do projeto:

    ```
    cd F1API
    ```

3. Configure as variáveis de ambiente:

    1. .env na raiz do projeto
    2. Configure o arquivo appsettings.json em ./F1RestAPI

4. Navegue até o diretório da API:

    ```
    cd F1RestAPI
    ```

5. Inicie o contêiner com o banco de dados:

    ```
    docker-compose up -d --build
    ```

6. Execute as migrações do Entity Framework:

    ```
    dotnet ef database update
    ```

7. Reinicie os contêineres:
    ```
    docker-compose up -d --build
    ```

## 📊 Monitoramento

As métricas do sistema são coletadas pelo Prometheus e podem ser visualizadas através do Grafana. As configurações podem ser ajustadas no arquivo prometheus.yml.
