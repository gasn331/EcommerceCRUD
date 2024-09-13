-- Altere o delimitador para permitir a criação do trigger
DELIMITER //

-- Trigger para Auditoria de Inserção de Produtos
CREATE TRIGGER trg_Produto_Insert
AFTER INSERT ON Produtos
FOR EACH ROW
BEGIN
    INSERT INTO ProdutoAudit (ProdutoId, Codigo, Descricao, DepartamentoId, Preco, Status, Excluido, ChangeType, ChangeDate)
    VALUES (NEW.Id, NEW.Codigo, NEW.Descricao, NEW.DepartamentoId, NEW.Preco, NEW.Status, NEW.Excluido, 'INSERT', NOW());
END; //

-- Restaure o delimitador para o padrão
DELIMITER ;


-- Altere o delimitador para permitir a criação do trigger
DELIMITER //

-- Trigger para Auditoria de Atualização de Produtos
CREATE TRIGGER trg_Produto_Update
AFTER UPDATE ON Produtos
FOR EACH ROW
BEGIN
    INSERT INTO ProdutoAudit (ProdutoId, Codigo, Descricao, DepartamentoId, Preco, Status, Excluido, ChangeType, ChangeDate)
    VALUES (NEW.Id, NEW.Codigo, NEW.Descricao, NEW.DepartamentoId, NEW.Preco, NEW.Status, NEW.Excluido, 'UPDATE', NOW());
END; //

-- Restaure o delimitador para o padrão
DELIMITER ;

-- Altere o delimitador para permitir a criação do trigger
DELIMITER //

-- Trigger para Auditoria de Exclusão de Produtos
CREATE TRIGGER trg_Produto_Delete
AFTER DELETE ON Produtos
FOR EACH ROW
BEGIN
    INSERT INTO ProdutoAudit (ProdutoId, Codigo, Descricao, DepartamentoId, Preco, Status, Excluido, ChangeType, ChangeDate)
    VALUES (OLD.Id, OLD.Codigo, OLD.Descricao, OLD.DepartamentoId, OLD.Preco, OLD.Status, OLD.Excluido, 'DELETE', NOW());
END; //

-- Restaure o delimitador para o padrão
DELIMITER ;
