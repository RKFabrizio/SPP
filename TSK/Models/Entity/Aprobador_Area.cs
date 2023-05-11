using SPP.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public class Aprobador_Area
    {
            public Aprobador_Area()
            {
                LoginAprobador = new HashSet<Pago>();
            }   

            public int IdAprobador { get; set; }

            public int IdArea { get; set; }
            public int IdUsuario { get; set; }

            public virtual Usuario IdAreaNavigation { get; set; }
            public virtual Area IdUsrNavigation { get; set; }
            public virtual ICollection<Pago> LoginAprobador { get; set; }
    }
 }
