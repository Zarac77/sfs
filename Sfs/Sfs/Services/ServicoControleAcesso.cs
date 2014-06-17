using System;
using System.Linq;

using Sfs.Models;
using System.Security.Cryptography;

namespace Sfs.Services
{
    public class ServicoControleAcesso : Servico
    {
        public static bool Login(string user, string senha, Pessoa pessoa) {
            var valida = CompararSHA1(HashSenha(user, senha), pessoa.Senha);
            return valida;
        }

        public static bool AlterarSenha(SfsContext context, Guid id, string senhaAtual, string novaSenha, string confirmacaoNovaSenha) {
            //
            // Verifica se houve erro na digitação da nova senha.
            if (novaSenha != confirmacaoNovaSenha)
                return false;

            var pessoa = context.Pessoas.Single(u => u.Id == id);
            //
            // Verifica se a senha atual informada está incorreta.
            var senhaHash = HashSenha(pessoa.Email, senhaAtual);
            if (!CompararSHA1(pessoa.Senha, senhaHash))
                return false;

            pessoa.Senha = HashSenha(pessoa.Email, novaSenha);
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

        public static byte[] HashSenha(string userId, string senha) {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(userId + senha));
        }

        public static bool CompararSHA1(byte[] p1, byte[] p2) {
            bool result = false;
            if (p1 != null && p2 != null) {
                if (p1.Length == p2.Length) {
                    result = true;
                    for (int i = 0; i < p1.Length; i++) {
                        if (p1[i] != p2[i]) {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}