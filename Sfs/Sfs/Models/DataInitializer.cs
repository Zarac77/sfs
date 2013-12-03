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
            new Perfil() { Id = Perfil.GUID_PERFIL_PROFESSOR, Nome = "Professor", Descricao = "Professores da ESEM" },
            new Perfil() { Id = Perfil.GUID_PERFIL_CONTROLE, Nome = "Controle", Descricao = "Agente de logística que controla as presenças nas atividades" }
        };

        private List<EstadoAtividade> estadosAtividade = new List<EstadoAtividade> {
             new EstadoAtividade { Id = Guid.NewGuid(), Indice = EstadoAtividade.Edicao, Descricao = "Em edição"},
             new EstadoAtividade { Id = Guid.NewGuid(), Indice = EstadoAtividade.Aberta, Descricao = "Aberta para inscrições/desistências"},
             new EstadoAtividade { Id = Guid.NewGuid(), Indice = EstadoAtividade.Validada, Descricao = "Validada - pronta para realização"},
             new EstadoAtividade { Id = Guid.NewGuid(), Indice = EstadoAtividade.Arquivada, Descricao = "Arquivada/Fechada"},
             new EstadoAtividade { Id = Guid.NewGuid(), Indice = EstadoAtividade.Cancelada, Descricao = "Cancelada"}
        };

        private void SeedPerfis(SfsContext context)
        {
            perfis.ForEach(p => context.Perfis.Add(p));
        }

        private void SeedEstadoAtividade(SfsContext context) {
            estadosAtividade.ForEach(e => context.EstadosAtividade.Add(e));
        }

        private void SeedPessoas(SfsContext context)
        {
            new List<Pessoa> {
                new Pessoa { Id = Guid.Parse("0E79C10F-B329-4BFE-BE32-E2FE6EFFC618"), Matricula = "0001", Nome = "Administrador", Email = @"admin@whatever.com.br", Senha = ServicoControleAcesso.HashSenha(@"admin@whatever.com.br", "adminesem"), Perfis = new List<Perfil> { perfis.Single(p => p.Id == Perfil.GUID_PERFIL_ADMINISTRADOR ) }}
            }.ForEach(a => context.Pessoas.Add(a));
        }

        private void SeedAtividades(SfsContext context)
        {
            new List<Atividade> {
                new Atividade { Id = Guid.NewGuid(), EstadoAtividade = estadosAtividade.Single(e => e.Indice == EstadoAtividade.Edicao), Descricao = "Uma saída bem feliz", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), EstadoAtividade = estadosAtividade.Single(e => e.Indice == EstadoAtividade.Edicao), Descricao = "Uma saída bem triste", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), EstadoAtividade = estadosAtividade.Single(e => e.Indice == EstadoAtividade.Edicao), Descricao = "Do you even want to go out", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) },
                new Atividade { Id = Guid.NewGuid(), EstadoAtividade = estadosAtividade.Single(e => e.Indice == EstadoAtividade.Edicao), Descricao = "Shopping", NumeroVagas = 15, DataHoraInicio = new DateTime(2013, 6, 28, 21, 2, 3), DataHoraFim = new DateTime(2013, 6, 29, 21, 2, 3), DataLimiteInscricao = new DateTime(2013, 6, 28, 21, 2, 3), DataLimiteCancelamento = new DateTime(2013, 12, 28, 21, 2, 3) }
            }.ForEach(a => context.Atividades.Add(a));
        }

        private void SeedInboxes(SfsContext context)
        {
            new List<Inbox> {
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
            SeedEstadoAtividade(context);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}