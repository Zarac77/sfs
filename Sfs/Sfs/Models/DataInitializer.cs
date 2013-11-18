using Sfs.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sfs.Models
{
    // public class DataInitializer : CreateDatabaseIfNotExists<SfsContext>
    //public class DataInitializer : DropCreateDatabaseAlways<SfsContext>
    public class DataInitializer : DropCreateDatabaseIfModelChanges<SfsContext>
    {
        private List<Perfil> perfis = new List<Perfil>
        {
            new Perfil() { Id = Perfil.GUID_PERFIL_ADMINISTRADOR, Nome = "Administrador", Descricao = "Administradores do sistema" },
            new Perfil() { Id = Perfil.GUID_PERFIL_ALUNO, Nome = "Aluno", Descricao = "Alunos da ESEM" },
            new Perfil() { Id = Perfil.GUID_PERFIL_PROFESSOR, Nome = "Professor", Descricao = "Professores da ESEM" }
        };

        private void SeedPerfis(SfsContext context)
        {
            perfis.ForEach(p => context.Perfis.Add(p));
        }

        private void SeedPessoas(SfsContext context)
        {
            new List<Pessoa> {
                //new Pessoa { Id = Guid.Parse("04725000-12AD-4C23-9D3F-64ECE28E6760"), Matricula = "2011.0004", Nome = "Westerbly Snaydley", Email = @"westerbly@gmail.com", Turma = "3C" },
                //new Pessoa { Id = Guid.Parse("DC8F560F-1BA7-4C3D-878B-02C97E388E09"), Matricula = "1255", Nome = "Italo Gomes", Email = @"igomes@sesc.com.br", Turma = "4U" },
                new Pessoa { Id = Guid.Parse("0E79C10F-B329-4BFE-BE32-E2FE6EFFC618"), Matricula = "0001", Nome = "Administrador", Email = @"admin@whatever.com.br", Senha = ServicoControleAcesso.HashSenha(@"admin@whatever.com.br", "adminesem"), Perfis = new List<Perfil> { perfis.Single(p => p.Id == Perfil.GUID_PERFIL_ADMINISTRADOR ) }}
            }.ForEach(a => context.Pessoas.Add(a));
        }

        private void SeedAtividades(SfsContext context)
        {
            new List<Atividade> {
                new Atividade { Id = Guid.NewGuid(), Validada = false, Descricao = "Uma saída bem feliz.", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), Validada = false, Descricao = "Uma saída bem triste.", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), Validada = false, Descricao = "Do you even want to go out?", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), Validada = false, Descricao = "Shopping :D", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) }
                //new Atividade { Id = new Guid(), Validada = true, Descricao = "Uma saída bem feliz.", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3) },
                //new Atividade { Id = new Guid(), Validada = true, Descricao = "Uma saída bem feliz.", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3) }
            }.ForEach(a => context.Atividades.Add(a));
        }

        private void SeedInboxes(SfsContext context)
        {
            new List<Inbox> {
                //new Inbox { Id = Guid.NewGuid(), IdPessoa = Guid.Parse("04725000-12AD-4C23-9D3F-64ECE28E6760")},
                //new Inbox { Id = Guid.NewGuid(), IdPessoa = Guid.Parse("DC8F560F-1BA7-4C3D-878B-02C97E388E09")},
                new Inbox { Id = Guid.NewGuid(), IdPessoa = Guid.Parse("0E79C10F-B329-4BFE-BE32-E2FE6EFFC618")}
            }.ForEach(a => context.Inboxes.Add(a));
        }

        private void SeedParametrosSistema(SfsContext context)
        {
            new List<ParametrosSistema> {
                new ParametrosSistema { Id = Guid.NewGuid(), PontuacaoInicialAlunos = 50}
            }.ForEach(a => context.ParametrosSistema.Add(a));
        }

        protected override void Seed(SfsContext context)
        {
            SeedParametrosSistema(context);
            SeedPerfis(context);            
            SeedPessoas(context);            
            SeedAtividades(context);           
            SeedInboxes(context);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}