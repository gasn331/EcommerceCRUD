-- Tabela de Auditoria para Produtos
CREATE TABLE ProdutoAudit (
    AuditId INT AUTO_INCREMENT PRIMARY KEY,
    ProdutoId CHAR(36),
    Codigo VARCHAR(10),
    Descricao VARCHAR(255),
    DepartamentoId CHAR(36),
    Preco DECIMAL(10, 2),
    Status BOOLEAN,
    Excluido BOOLEAN,
    ChangeType ENUM('INSERT', 'UPDATE', 'DELETE'),
    ChangeDate DATETIME
);
