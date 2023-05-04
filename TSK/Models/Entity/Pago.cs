using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Pago
    {
        public Pago()
        {
            EnvioCorreos = new HashSet<EnvioCorreo>();

        }
        public int IdPago { get; set; }
        public int IdTipoAdelanto { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int IdTipoMoneda { get; set; }
        public float Importe { get; set; }
        public string Concepto { get; set; }
        public int LoginSolicitante { get; set; }
        public int LoginAprobador { get; set; }
        public string ReferenciaOC { get; set; }
        public string ProformaCotizacion { get; set; }
        public string Factura { get; set; }
        public int IdTipoPago { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public int IdEstado { get; set; }
        public string InformacionContable { get; set; }

        public virtual Tipo_Adelanto TipoAdelantoNavigation { get; set; }
        public virtual Proveedor ProveedorNavigation { get; set; }
        public virtual Tipo_Moneda TipoMonedaNavigation { get; set; }
        public virtual Usuario SolicitanteNavigation { get; set; }
        public virtual Usuario AprobadorNavigation { get; set; }
        public virtual Tipo_Pago TipoPagoNavigation { get; set; }
        public virtual Estado EstadoNavigation { get; set; }
        public virtual ICollection<EnvioCorreo> EnvioCorreos { get; set; }
    }

}
