using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IceCreamCompany.Domain.Entities.Abstract;

namespace IceCreamCompany.Domain.Entities
{
    public class Workflow : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WorkflowId { get; set; }

        public string WorkflowName { get; set; }

        public bool IsActive { get; set; }

        public string MultiExecBehavior { get; set; }
    }
}