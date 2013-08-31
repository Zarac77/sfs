using System;
using System.Linq;

using Sfs.Models;

namespace Sfs.Services
{
    public class ServicoControleAcesso
    {
        public static bool AlterarSenha(SfsContext context, Guid id, string senhaAtual, 
            string novaSenha, string confirmacaoNovaSenha)
        {
            //
            // Verifica se hovue erro na digitação da nova senha.
            if (novaSenha != confirmacaoNovaSenha)
                return false;

            var pessoa = context.Pessoas.Single(u => u.Id == id);
            //
            // Verifica se a senha atual informada está incorreta.
            if (pessoa.Senha != senhaAtual)
                return false;

            pessoa.Senha = novaSenha;
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Verifica se a pessoa possui determinado perfil de acesso.
        /// </summary>
        /// <param name="context">Classe de contexto.</param>
        /// <param name="idPessoa">Pessoa a ser verificada.</param>
        /// <param name="idPerfil">Perfil de acesso a ser verificado.</param>
        /// <returns>True se a pessoa possuir o perfil informado.</returns>
        public static bool VerificarPerfil(SfsContext context, Guid idPessoa, Guid idPerfil)
        {
            var pessoa = context.Pessoas.Find(idPessoa);
            var perfil = context.Perfis.Find(idPerfil);
            return pessoa.Perfis.Contains(perfil);
        }
    }
}