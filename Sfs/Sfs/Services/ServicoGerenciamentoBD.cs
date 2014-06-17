using Sfs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Sfs.Services {
    public class ServicoGerenciamentoBD : Servico {
        /// <summary>
        /// À partir de um arquivo CSV, importa os alunos da instituição.
        ///  AVISO: Extremamente dependente da formatação do arquivo, que deve ser:
        ///  NOME_DA_PESSOA;MATRÍCULA;----;TURMA;ANO_DE_ENTRADA;E-MAIL
        /// </summary>
        /// <param name="context">Uma instância da classe de contexto do sistema.</param>
        /// <param name="source">Arquivo csv importado.</param>
        /// <returns></returns>
        public static bool AlimentarBD(SfsContext context, HttpPostedFileBase source) {
            var stream = source.InputStream;
            var buf = new byte[stream.Length];
            if (stream.Length < int.MaxValue) {
                stream.Read(buf, 0, (int)stream.Length);
            }
            var textoFile = Encoding.UTF8.GetString(buf);
            var reader = new StringReader(textoFile);
            var linha = reader.ReadLine();
            List<Pessoa> pessoas = new List<Pessoa>();
            while (linha != null) {
                string[] dados = linha.Split(';');
                Pessoa p = new Pessoa {
                    Id = Guid.NewGuid(),
                    Nome = dados[0],
                    Matricula = dados[1],
                    Turma = dados[3],
                    AnoEntrada = int.Parse(dados[4]),
                    Email = string.IsNullOrEmpty(dados[5]) ? Guid.NewGuid() + "@escolasesc.g12.br" : dados[5],
                    Perfis = new List<Perfil> { context.Perfis.Single(pf => pf.Id == Perfil.GUID_PERFIL_ALUNO) },
                    PreSenha = "123456"
                };
                pessoas.Add(p);
                linha = reader.ReadLine();
            }
            pessoas.ForEach(p => ServicoPessoa.RegistrarPessoa(context, p));
            context.SaveChanges();
            return true;
        }
    }
}