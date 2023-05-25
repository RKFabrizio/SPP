using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SPP.Models.Entity;

namespace SPP.Models.Entity
{
    public partial class SPPEU2GIGDEVSQLContext : DbContext
    {
        public SPPEU2GIGDEVSQLContext()
        {
        }

        public SPPEU2GIGDEVSQLContext(DbContextOptions<SPPEU2GIGDEVSQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }
        public virtual DbSet<Compania> Companias { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet <EnvioCorreo> EnvioCorreos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Tipo_Moneda> TipoMonedas { get; set; }
        public virtual DbSet<Tipo_Adelanto> TipoAdelantos { get; set; }
        public virtual DbSet<Tipo_Cuenta> TipoCuentas { get; set; }
        public virtual DbSet<Tipo_Documento> Tipo_Documentos { get; set; }
        public virtual DbSet<Tipo_Pago> TipoPagos { get; set; }
        public virtual DbSet<Aprobador_Area> AprobadorAreas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //    optionsBuilder.UseSqlServer("Server=LAPTOP-5GJJMNSE;Database=USAEU2GIG-DEV-SQL;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Aprobador_Area>(entity =>
            {
                entity.HasKey(e => e.IdAprobador);

                entity.Property(e => e.IdAprobador)
                    .HasColumnName("ID_APROBADOR");

                entity.ToTable("APROBADOR_AREA");

                entity.Property(e => e.IdArea)
                    .HasColumnName("ID_AREA");

                entity.Property(e => e.IdUsuario)
                     .HasColumnName("ID_USUARIO");


                entity.HasOne(d => d.IdAreaNavigation)
                    .WithMany(p => p.AprobadorAreas)
                    .HasForeignKey(d => d.IdArea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APROBADOR_AREA_AREA");
                
                entity.HasOne(d => d.IdUsrNavigation)
                    .WithMany(p => p.AprobadorAreas)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APROBADOR_AREA_USUARIO");
            });


            modelBuilder.Entity<Tipo_Moneda>(entity =>
            {
                entity.HasKey(e => e.IdTipoMoneda);

                entity.ToTable("TIPO_MONEDA");

                entity.Property(e => e.IdTipoMoneda).HasColumnName("IDTIPOMONEDA");

                entity.Property(e => e.TipoMoneda)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TIPOMONEDA");

                entity.Property(e => e.Simbologia)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SIMBOLOGIA");
            });

            modelBuilder.Entity<Tipo_Cuenta>(entity =>
            {
                entity.HasKey(e => e.IdTipoCuenta);

                entity.ToTable("TIPO_CUENTA");

                entity.Property(e => e.IdTipoCuenta).HasColumnName("IDTIPOCUENTA");

                entity.Property(e => e.TipoCuenta)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TIPOCUENTA");

            });

            modelBuilder.Entity<Tipo_Documento>(entity =>
            {
                entity.HasKey(e => e.IdTipoDocumento);

                entity.ToTable("TIPO_DOCUMENTO");

                entity.Property(e => e.IdTipoDocumento).HasColumnName("IDTIPODOCUMENTO");

                entity.Property(e => e.NombreDocumento)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREDOCUMENTO");
            });

            modelBuilder.Entity<Tipo_Adelanto>(entity =>
            {
                entity.HasKey(e => e.IdTipoAdelanto);

                entity.ToTable("TIPO_ADELANTO");

                entity.Property(e => e.IdTipoAdelanto).HasColumnName("IDTIPOADELANTO");

                entity.Property(e => e.TipoAdelanto)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TIPOADELANTO");
            });

            modelBuilder.Entity<Tipo_Pago>(entity =>
            {
                entity.HasKey(e => e.IdTipoPago);

                entity.ToTable("TIPO_PAGO");

                entity.Property(e => e.IdTipoPago).HasColumnName("IDTIPOPAGO");

                entity.Property(e => e.TipoPago)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TIPOPAGO");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.ToTable("ESTADO");

                entity.Property(e => e.IdEstado).HasColumnName("IDESTADO");

                entity.Property(e => e.NombreEstado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREESTADO");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.ToTable("PERFIL");

                entity.Property(e => e.IdPerfil).HasColumnName("IDPERFIL");

                entity.Property(e => e.NombrePerfil)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREPERFIL");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.IdArea);

                entity.ToTable("AREA");

                entity.Property(e => e.IdArea).HasColumnName("IDAREA");

                entity.Property(e => e.NombreArea)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREAREA");
            });

            modelBuilder.Entity<Banco>(entity =>
            {
                entity.HasKey(e => e.IdBanco);

                entity.ToTable("BANCO");

                entity.Property(e => e.IdBanco).HasColumnName("IDBANCO");

                entity.Property(e => e.NombreBanco)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREBANCO");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("USUARIO");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(50)
                    .HasColumnName("IDUSUARIO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("APELLIDO");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("CORREO");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CONTRASENA");

                entity.Property(e => e.Login)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("LOGIN");


                entity.Property(e => e.MontoAprobacion).HasColumnName("MONTOAPROBACION");
                entity.Property(e => e.IdPerfil).HasColumnName("IDPERFIL");
                entity.Property(e => e.IdArea).HasColumnName("IDAREA");
                entity.Property(e => e.IdCompania).HasColumnName("IDCOMPANIA");
                entity.Property(e => e.Habilitado).HasColumnName("HABILITADO");

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("TOKEN");

                entity.HasOne(d => d.PerfilDisNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPerfil)
                    .HasConstraintName("FK_USUARIO_PERFIL");

                entity.HasOne(d => d.AreaDisNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdArea)
                    .HasConstraintName("FK_USUARIO_AREA");

                entity.HasOne(d => d.CompaniaDisNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdCompania)
                    .HasConstraintName("FK_USUARIO_COMPANIA");
            });

            // Continúa con el resto de las entidades

            modelBuilder.Entity<Pais>(entity =>
            {
                entity.HasKey(e => e.IdPais);

                entity.ToTable("PAIS");

                entity.Property(e => e.IdPais).HasColumnName("IDPAIS");

                entity.Property(e => e.NombrePais)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREPAIS");
            });

            modelBuilder.Entity<Compania>(entity =>
            {
                entity.HasKey(e => e.IdCompania);

                entity.ToTable("COMPANIA");

                entity.Property(e => e.IdCompania).HasColumnName("IDCOMPANIA");

                entity.Property(e => e.NombreCompania)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRECOMPANIA");
            });


            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor);

                entity.ToTable("PROVEEDOR");

                entity.Property(e => e.IdProveedor).HasColumnName("IDPROVEEDOR");

                entity.Property(e => e.NombreProveedor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBREPROVEEDOR");

                entity.Property(e => e.IdPais)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("IDPAIS");

                entity.HasOne(d => d.IdDisPais)
                    .WithMany(p => p.Proveedores)
                    .HasForeignKey(d => d.IdPais)
                    .HasConstraintName("FK_PROVEEDOR_PAIS");

                entity.Property(e => e.IdTipoDoc)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnName("IDTIPODOC");

                entity.HasOne(d => d.IdDisDocumento)
                    .WithMany(p => p.Proveedores)
                    .HasForeignKey(d => d.IdTipoDoc)
                    .HasConstraintName("FK_PROVEEDOR_TIPO_DOCUMENTO");

            });

            modelBuilder.Entity<EnvioCorreo>(entity =>
            {
                entity.HasKey(e => e.IdCorreo);

                entity.ToTable("ENVIOCORREO");

                entity.Property(e => e.IdCorreo).HasColumnName("IDCORREO");

                entity.Property(e => e.NombreCorreo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("NOMBRECORREO");

                entity.Property(e => e.IdPago)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("IDPAGO");

                entity.HasOne(d => d.PagoDisNavigation)
                    .WithMany(p => p.EnvioCorreos)
                    .HasForeignKey(d => d.IdPago)
                    .HasConstraintName("FK_ENVIO_CORREO_PAGO");

            });



                modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.IdPago);

                entity.ToTable("PAGO");

                entity.Property(e => e.IdPago).HasColumnName("IDPAGO");

                entity.Property(e => e.IdTipoAdelanto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDTIPOADELANTO")
                    .IsFixedLength();

                entity.Property(e => e.IdProveedor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDPROVEEDOR")
                    .IsFixedLength();

                entity.Property(e => e.FechaSolicitud)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FECHASOLICITUD")
                    .IsFixedLength();

                entity.Property(e => e.IdTipoMoneda)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDTIPOMONEDA")
                    .IsFixedLength();

                entity.Property(e => e.Importe)
                    .HasColumnName("IMPORTE")
                    .HasColumnType("float");


                entity.Property(e => e.Concepto)
                    .IsRequired(false)
                    .HasMaxLength(50)
                    .HasColumnName("CONCEPTO");

                entity.Property(e => e.LoginSolicitante)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGINSOLICITANTE")
                    .IsFixedLength();

                entity.Property(e => e.LoginAprobador)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOGINAPROBADOR")
                    .IsFixedLength();

                entity.Property(e => e.ReferenciaOC)
                    .IsRequired(false)
                    .HasMaxLength(50)
                    .HasColumnName("REFERENCIAOC");

                entity.Property(e => e.ProformaCotizacion)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("PROFORMACOTIZACION");
                   

                entity.Property(e => e.Factura)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("FACTURA");

                entity.Property(e => e.IdTipoPago)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("IDTIPOPAGO");

                entity.Property(e => e.Observaciones)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("OBSERVACIONES");

                entity.Property(e => e.FechaAprobacion)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("FECHAAPROBACION");

                entity.Property(e => e.IdEstado)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnName("IDESTADO")
                   .IsFixedLength();

                entity.Property(e => e.InformacionContable)
                  .IsRequired(false)
                  .HasMaxLength(50)
                  .HasColumnName("INFORMACIONCONTABLE");

                entity.Property(e => e.IdBanco)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("IDBANCO");

                entity.Property(e => e.IdTipoCuenta)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("IDTIPOCUENTA");

                entity.Property(e => e.IdTipoPago)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("IDTIPOPAGO");

                entity.Property(e => e.BeneficiarioNombre)
                 .IsRequired(false)
                 .HasMaxLength(50)
                 .HasColumnName("BENEFICIARIONOMBRE");

                entity.Property(e => e.BeneficiarioDni)
                   .IsRequired(false)
                   .HasMaxLength(50)
                   .HasColumnName("BENEFICIARIODNI");

                entity.Property(e => e.CuentaBancaria)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnName("CUENTABANCARIA");


                entity.HasOne(d => d.ProveedorNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdProveedor)
                    .HasConstraintName("FK_PAGO_PROVEEDOR");

                entity.HasOne(d => d.TipoAdelantoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdTipoAdelanto)
                    .HasConstraintName("FK_PAGO_TIPO_ADELANTO");

                entity.HasOne(d => d.TipoMonedaNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdTipoMoneda)
                    .HasConstraintName("FK_PAGO_TIPO_MONEDA");

                entity.HasOne(d => d.SolicitanteNavigation)
                    .WithMany(p => p.LoginSolicitante)
                    .HasForeignKey(d => d.LoginSolicitante)
                    .HasConstraintName("FK_PAGO_USUARIO_SOLICITANTE");

                entity.HasOne(d => d.AprobadorNavigation)
                    .WithMany(p => p.LoginAprobador)
                    .HasForeignKey(d => d.LoginAprobador)
                    .HasConstraintName("FK_PAGO_USUARIO_APROBADOR");

                entity.HasOne(d => d.TipoPagoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdTipoPago)
                    .HasConstraintName("FK_PAGO_TIPO_PAGO");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdEstado)
                    .HasConstraintName("FK_PAGO_ESTADO");

                entity.HasOne(d => d.BancoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdBanco)
                    .HasConstraintName("FK_PAGO_BANCO");

                entity.HasOne(d => d.TipoCuentaNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdTipoCuenta)
                    .HasConstraintName("FK_PAGO_TIPO_CUENTA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
