SELECT
    Assunto,
    Ano,
    COUNT(*) AS Quantidade
FROM
    atendimentos
GROUP BY
    Assunto,
    Ano
HAVING
    COUNT(*) > 3
ORDER BY
    Ano DESC,
    Quantidade DESC;