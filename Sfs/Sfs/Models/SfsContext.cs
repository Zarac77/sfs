using System.Data.Entity;

namespace Sfs.Models
{
    public class SfsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Sfs.Models.SfsContext>());

        public SfsContext() : base("name=SfsContext")
        {
        }

        public DbSet<Atividade> Atividades { get; set; }

        public DbSet<Pessoa> Pessoas { get; set; }

        public DbSet<Aluno> Alunos { get; set; }

        public DbSet<Professor> Professores { get; set; }

        public DbSet<Perfil> Perfis { get; set; }

        public DbSet<Inscricao> Inscricoes { get; set; }

        public DbSet<Inbox> Inboxes { get; set; }

        public DbSet<Mensagem> Mensagens { get; set; }
    }
}
