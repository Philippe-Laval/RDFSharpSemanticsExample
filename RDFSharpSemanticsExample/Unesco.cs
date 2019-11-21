using RDFSharp.Model;
using RDFSharp.Semantics;
using RDFSharp.Semantics.LinkedData.SKOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSemantics
{
    public class Unesco
    {
        public void Load()
        {
            //RDFOntology ont = RDFSKOSOntology.Instance;

            //ont.Data.From

            //var graph = ont.Data.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            //RDFGraph graph = RDFGraph.FromFile(RDFModelEnums.RDFFormats.Turtle, @"C:\Users\philippel\Documents\ONU\unescothes.ttl");
            RDFGraph graph = RDFGraph.FromFile(RDFModelEnums.RDFFormats.RdfXml, @"C:\Users\philippel\Documents\ONU\unescothes.rdf");

            long triplesCount = graph.TriplesCount;

        }
    }
}
