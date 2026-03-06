// Totais de uma pessoa (receitoas/gastos/total)
export interface TotaisPorPessoa {
  pessoaId: number;
  pessoaNome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

// Relatorio Total por pessoa e os totais gerais
export interface RelatorioTotaisPessoa {
  totaisPorPessoa: TotaisPorPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquido: number;
}

// Totais por categoria
export interface TotaisPorCategoria {
  categoriaId: number;
  categoriaDescricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

// Relatório de cada categoria e os totais
export interface RelatorioTotaisCategoria {
  totaisPorCategoria: TotaisPorCategoria[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoLiquido: number;
}
