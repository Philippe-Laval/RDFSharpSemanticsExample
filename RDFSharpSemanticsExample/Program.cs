using RDFSharp.Model;
using RDFSharp.Query;
using RDFSharp.Semantics;
using RDFSharp.Semantics.LinkedData.DC;
using RDFSharp.Semantics.LinkedData.FOAF;
using RDFSharp.Semantics.LinkedData.GEO;
using RDFSharp.Semantics.LinkedData.SIOC;
using RDFSharp.Semantics.LinkedData.SKOS;
using RDFSharp.Semantics.Reasoner;
using RDFSharp.Semantics.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://www.w3.org/TR/2014/REC-json-ld-20140116/


namespace TestSemantics
{
    class Program
    {
        static void Main(string[] args)
        {
            // SUBSCRIBE INFORMATIVE MESSAGES (E.G.: LOG TO CONSOLLE)
            RDFSemanticsEvents.OnSemanticsInfo += ((msg) => Console.WriteLine(msg));
            // SUBSCRIBE WARNING MESSAGES (E.G.: LOG TO CONSOLLE)
            RDFSemanticsEvents.OnSemanticsWarning += ((msg) => Console.WriteLine(msg));

            //Unesco unesco = new Unesco();
            //unesco.Load();

            // 1.1 About this Primer

            /*
@prefix skos: <http://www.w3.org/2004/02/skos/core#> .
@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .
@prefix owl: <http://www.w3.org/2002/07/owl#> .
@prefix dct: <http://purl.org/dc/terms/> .
@prefix foaf: <http://xmlns.com/foaf/0.1/> .
@prefix ex: <http://www.example.com/> .
@prefix ex1: <http://www.example.com/1/> .
@prefix ex2: <http://www.example.com/2/> .
             */


            RDFNamespaceRegister.AddNamespace(new RDFNamespace("skos", "http://www.w3.org/2004/02/skos/core#"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace(RDFVocabulary.SKOS.SKOSXL.PREFIX, RDFVocabulary.SKOS.SKOSXL.BASE_URI));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("owl", "http://www.w3.org/2002/07/owl#"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("dct", "http://purl.org/dc/terms/"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("foaf", "http://xmlns.com/foaf/0.1/"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("ex", "http://www.example.com/"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("ex1", "http://www.example.com/1/"));
            RDFNamespaceRegister.AddNamespace(new RDFNamespace("ex2", "http://www.example.com/2/"));


            SkosPrimer();
            return;

            


            // Examples from the documentation to learn the concept and how to use them with the API
            CreateOntology();

            // Try to use the RDFSharp.Semantics.LinkedData
            //
            // First we need to understand the OWL way to create an ontology.
            // This assembly contains different ontologies already build for us.
            // They are good exemples on how to create ontology.

            UseLinkedDataOntologies();



        }

        static void CreateOntology()
        { 
            // CREATE ONTOLOGY
            var ont = new RDFOntology(new RDFResource("http://cats_ont/v7/"));

            // CREATE OWL ONTOLOGY CLASS
            var cat = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/cat"));
            // CREATE RDFS ONTOLOGY CLASS
            var cat_rdfs = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/cat_rdfs"),
                                    RDFSemanticsEnums.RDFOntologyClassNature.RDFS);

            ont.Model.ClassModel.AddClass(cat);
            ont.Model.ClassModel.AddClass(cat_rdfs);

            #region AddSubClassOfRelation
            // CREATE ONTOLOGY CLASS
            var siberian = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/siberian"));
            // ADD ONTOLOGY CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(siberian);
            // ADD SUBCLASSOF RELATION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddSubClassOfRelation(siberian, cat);
            #endregion

            #region AddEquivalentClassRelation
            // CREATE ONTOLOGY CLASS
            var siberianCat = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/siberianCat"));
            // ADD ONTOLOGY CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(siberianCat);
            // ADD EQUIVALENTCLASS RELATION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddEquivalentClassRelation(siberianCat, siberian);
            #endregion

            #region AddDisjointWithRelation
            // CREATE ONTOLOGY CLASS
            var persian = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/persian"));
            // ADD ONTOLOGY CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(persian);
            // ADD DISJOINTWITH RELATION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddDisjointWithRelation(persian, siberian);
            #endregion

            #region AddUnionOfRelation
            // CREATE ONTOLOGY UNION CLASS
            var oriental = new RDFOntologyUnionClass(new RDFResource("http://cats_ont/v7/model/classes/oriental"));
            // ADD ONTOLOGY UNION CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(oriental);
            // ADD UNIONOF RELATIONS TO ONTOLOGY MODEL
            var orientalCats = new List<RDFOntologyClass>() { persian, siberian };
            ont.Model.ClassModel.AddUnionOfRelation(oriental, orientalCats);
            #endregion

            #region AddIntersectionOfRelation
            // CREATE ONTOLOGY INTERSECTION CLASS
            var white = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/white"));
            var glacial = new RDFOntologyIntersectionClass(new RDFResource("http://cats_ont/v7/model/classes/glacial"));
            // ADD ONTOLOGY INTERSECTION CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(white);
            ont.Model.ClassModel.AddClass(glacial);
            // ADD INTERSECTIONOF RELATIONS TO ONTOLOGY MODEL
            var glacialCats = new List<RDFOntologyClass>() { white, siberian };
            ont.Model.ClassModel.AddIntersectionOfRelation(glacial, glacialCats);
            #endregion

            #region RDFOntologyComplementClass
            // CREATE ONTOLOGY COMPLEMENT CLASS
            var nonPersian = new RDFOntologyComplementClass(new RDFResource("http://cats_ont/v7/model/classes/nonPersian"), persian);
            // ADD ONTOLOGY COMPLEMENT CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(nonPersian);
            #endregion

            #region RDFOntologyEnumerateClass - AddOneOfRelation
            // CREATE ONTOLOGY ENUMERATE CLASS
            var catsEnum = new RDFOntologyEnumerateClass(new RDFResource("http://cats_ont/v7/model/classes/catsEnum"));
            // ADD ONTOLOGY ENUMERATE CLASS TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddClass(catsEnum);
            // CREATE ONTOLOGY FACT
            var garfield = new RDFOntologyFact(new RDFResource("http://cats_ont/v7/data/facts/garfield"));
            // ADD ONTOLOGY FACT TO ONTOLOGY DATA
            ont.Data.AddFact(garfield);
            // ADD ONEOF RELATIONS TO ONTOLOGY MODEL
            var myCats = new List<RDFOntologyFact>() { garfield };
            ont.Model.ClassModel.AddOneOfRelation(catsEnum, myCats);
            #endregion

            // DEPRECATE ONTOLOGY CLASS
            cat_rdfs.SetDeprecated(true);

            // A restriction is a virtual class obtained by constraining the behavior of a specific
            // property, modeled as derived type of RDFOntologyRestriction.The property on which
            // the restriction is applied is exposed through the OnProperty property.
            // RDFSharp.Semantics provides support for 4 built-in restriction types.
            //
            // Since restrictions are classes, they can be used in class operations, compositions and
            // taxonomies: the main differences against classes are that restrictions cannot be
            // assigned as class types of facts and restrictions cannot be deprecated.

            #region RDFOntologyCardinalityRestriction
            // CREATE ONTOLOGY PROPERTY TO BE RESTRICTED
            var hasCat = new RDFOntologyObjectProperty(
                new RDFResource("http://cats_ont/v7/model/props/hasCat"));

            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasCat);

            // CREATE ONTOLOGY CARDINALITY RESTRICTION
            var catOwners = new RDFOntologyCardinalityRestriction(
                new RDFResource("http://cats_ont/v7/model/restricts/catOwners"), hasCat, 1, 0);

            // ADD ONTOLOGY RESTRICTION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddRestriction(catOwners);
            #endregion

            #region RDFOntologyHasValueRestriction
            // CREATE ONTOLOGY HASVALUE RESTRICTION
            var garfieldOwners = new RDFOntologyHasValueRestriction(
                new RDFResource("http://cats_ont/v7/model/restricts/garfieldOwners"), hasCat, garfield);
            // ADD ONTOLOGY RESTRICTION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddRestriction(garfieldOwners);
            #endregion

            #region RDFOntologyAllValuesFromRestriction
            // CREATE ONTOLOGY PROPERTY TO BE RESTRICTED
            var hasPet = new RDFOntologyObjectProperty(new RDFResource("http://cats_ont/v7/model/props/hasPet"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasPet);

            // CREATE ONTOLOGY ALLVALUESFROM RESTRICTION
            var catLovers = new RDFOntologyAllValuesFromRestriction(
                new RDFResource("http://cats_ont/v7/model/restricts/catLovers"), hasPet, cat);
            // ADD ONTOLOGY RESTRICTION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddRestriction(catLovers);
            #endregion

            #region RDFOntologySomeValuesFromRestriction
            // CREATE ONTOLOGY SOMEVALUESFROM RESTRICTION
            var catOwners2 = new RDFOntologySomeValuesFromRestriction(
                new RDFResource("http://cats_ont/v7/model/restricts/catOwners2"), hasPet, cat);
            // ADD ONTOLOGY RESTRICTION TO ONTOLOGY MODEL
            ont.Model.ClassModel.AddRestriction(catOwners2);
            #endregion

            #region RDFOntologyObjectProperty
            // CREATE ONTOLOGY OBJECT PROPERTY
            //var hasPet = new RDFOntologyObjectProperty(new RDFResource("http://cats_ont/v7/model/props/hasPet"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            //ont.Model.PropertyModel.AddProperty(hasPet);
            #endregion

            #region RDFOntologyDatatypeProperty
            // CREATE ONTOLOGY DATATYPE PROPERTY
            var hasName = new RDFOntologyDatatypeProperty(new
            RDFResource("http://cats_ont/v7/model/props/hasName"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasName);
            #endregion

            #region RDFOntologyAnnotationProperty
            // CREATE ONTOLOGY ANNOTATION PROPERTY
            var hasNote = new RDFOntologyAnnotationProperty(new
            RDFResource("http://cats_ont/v7/model/props/hasNote"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasNote);
            #endregion

            // CREATE ONTOLOGY PROPERTY
            var hasWife = new RDFOntologyObjectProperty(new RDFResource("http://ont/v12/model/props/hasWife"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasWife);
            // DEFINE THE PROPERTY AS FUNCTIONAL
            hasWife.SetFunctional(true);

            // CREATE ONTOLOGY PROPERTY
            var isWifeOf = new RDFOntologyObjectProperty(new RDFResource("http://ont/v12/model/props/isWifeOf"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(isWifeOf);
            // DEFINE THE PROPERTY AS FUNCTIONAL
            isWifeOf.SetInverseFunctional(true);

            // CREATE ONTOLOGY PROPERTY
            var hasBrother = new RDFOntologyObjectProperty(new RDFResource("http://ont/v12/model/props/hasBrother"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasBrother);
            // DEFINE THE PROPERTY AS SYMMETRIC
            hasBrother.SetSymmetric(true);

            // CREATE ONTOLOGY PROPERTY
            var hasSister = new RDFOntologyObjectProperty(new
            RDFResource("http://ont/v12/model/props/hasSister"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasSister);
            // DEFINE THE PROPERTY AS TRANSITIVE
            hasSister.SetTransitive(true);


            // CREATE DOMAIN/RANGE CLASSES
            var human = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/human"));
            var animal = new RDFOntologyClass(new RDFResource("http://cats_ont/v7/model/classes/animal"));

            ont.Model.ClassModel.AddClass(human);
            ont.Model.ClassModel.AddClass(animal);

            // ADJUST CLASS TAXONOMY RELATIONS
            ont.Model.ClassModel.AddSubClassOfRelation(cat, animal);
            // SPECIFY DOMAIN CLASS OF PROPERTY
            hasPet.SetDomain(human);
            // SPECIFY RANGE CLASS OF PROPERTY
            hasPet.SetRange(animal);

            // DEPRECATE ONTOLOGY PROPERTY
            // hasPet.SetDeprecated(true);

            #region AddSubPropertyOfRelation
            // CREATE ONTOLOGY PROPERTY
            //var hasCat = new RDFOntologyObjectProperty(
            //    new RDFResource("http://cats_ont/v7/model/properties/hasCat"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasCat);
            // ADD SUBPROPERTYOF RELATION TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddSubPropertyOfRelation(hasCat, hasPet);
            #endregion

            #region AddEquivalentPropertyRelation
            // CREATE ONTOLOGY PROPERTY
            var hasPet2 = new RDFOntologyObjectProperty(new
            RDFResource("http://cats_ont/v7/model/properties/hasPet2"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(hasPet2);
            // ADD EQUIVALENTPROPERTY RELATION TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddEquivalentPropertyRelation(hasPet2, hasPet);
            #endregion

            #region AddInverseOfRelation
            // CREATE ONTOLOGY PROPERTY
            var isPetOf = new RDFOntologyObjectProperty(new
            RDFResource("http://cats_ont/v7/model/properties/isPetOf"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(isPetOf);
            // ADD EQUIVALENTPROPERTY RELATION TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddInverseOfRelation(isPetOf, hasPet);
            #endregion


            // CREATE ONTOLOGY FACT
            //var garfield = new RDFOntologyFact(new RDFResource("http://cats_ont/v7/data/facts/garfield"));
            // ADD ONTOLOGY FACT TO ONTOLOGY DATA
            //ont.Data.AddFact(garfield);

            #region AddClassTypeRelation
            // ADD TYPE RELATION TO ONTOLOGY DATA
            ont.Data.AddClassTypeRelation(garfield, cat);
            #endregion

            #region AddSameAsRelation
            // CREATE ONTOLOGY FACT
            var myGarfield = new RDFOntologyFact(new
            RDFResource("http://cats_ont/v7/data/facts/myGarfield"));
            // ADD ONTOLOGY FACT TO ONTOLOGY DATA
            ont.Data.AddFact(myGarfield);
            // ADD SAMEAS RELATION TO ONTOLOGY DATA
            ont.Data.AddSameAsRelation(myGarfield, garfield);
            #endregion

            #region AddDifferentFromRelation
            // CREATE ONTOLOGY FACT
            var felix = new RDFOntologyFact(new RDFResource("http://cats_ont/v7/data/facts/felix"));
            // ADD ONTOLOGY FACT TO ONTOLOGY DATA
            ont.Data.AddFact(felix);
            // ADD DIFFERENTFROM RELATION TO ONTOLOGY DATA
            ont.Data.AddDifferentFromRelation(felix, garfield);
            #endregion

            // CREATE ONTOLOGY FACTS
            var mark = new RDFOntologyFact(new RDFResource("http://cats_ont/v7/data/facts/mark"));
            var tony = new RDFOntologyFact(new RDFResource("http://cats_ont/v7/data/facts/tony"));

            // CREATE ONTOLOGY LITERALS
            var markName = new RDFOntologyLiteral(new RDFPlainLiteral("Mark", "en-US"));
            var tonyName = new RDFOntologyLiteral(new RDFPlainLiteral("Tony", "en-US"));
            
            // ADD ONTOLOGY FACTS TO ONTOLOGY DATA
            ont.Data.AddFact(mark);
            ont.Data.AddFact(tony);
            
            // ADD TYPE RELATIONS TO ONTOLOGY DATA
            ont.Data.AddClassTypeRelation(mark, human);
            ont.Data.AddClassTypeRelation(tony, human);
            
            // ADD ASSERTIONS TO ONTOLOGY DATA
            ont.Data.AddAssertionRelation(mark, hasPet, garfield);
            ont.Data.AddAssertionRelation(mark, hasName, markName);
            ont.Data.AddAssertionRelation(tony, hasPet, felix);
            ont.Data.AddAssertionRelation(tony, hasName, tonyName);

            #region AddStandardAnnotation
            var comment = new RDFOntologyLiteral(new RDFPlainLiteral("A comment about Mark", "en-US"));

            ont.Data.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Comment,
                mark, comment);

            ont.Model.ClassModel.AddStandardAnnotation(RDFSemanticsEnums.RDFOntologyStandardAnnotation.Label,
                cat, new RDFOntologyLiteral(new RDFPlainLiteral("Label for cat class")));

            #endregion

            #region AddCustomAnnotation
            // CREATE ONTOLOGY ANNOTATION PROPERTY
            var ap = new RDFOntologyAnnotationProperty(new RDFResource("http://ont/props/my_annotation_prop"));
            // ADD ONTOLOGY PROPERTY TO ONTOLOGY MODEL
            ont.Model.PropertyModel.AddProperty(ap);
            // ADD CUSTOM ANNOTATION
            ont.Model.ClassModel.AddCustomAnnotation(ap, cat, new RDFOntologyLiteral(new RDFPlainLiteral("cat")));
            #endregion

           

            ValidateOntology(ont);

            // ONTOLOGY: REASONER
            ReasonOnOntology(ref ont);


            QueryOntology(ont);


            // Create a graph from the ontology
            var graph = ont.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);

            // Write the graph to disk as a turtle file
            graph.ToFile(RDFModelEnums.RDFFormats.Turtle, @"ont.ttl");

            graph.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"ont.rdf");


            // Load the turtle file into a graph
            var graph2 = RDFGraph.FromFile(RDFModelEnums.RDFFormats.Turtle, @"ont.ttl");

            // GET AN ONTOLOGY FROM A GRAPH
            var ont_new = RDFOntology.FromRDFGraph(graph2);

            var difference_ontology = ont.DifferenceWith(ont_new);

        }

        static void UseLinkedDataOntologies()
        {
            RDFOntology ont = RDFDCOntology.Instance;
            GenerateOntologyFiles(ont, @"RdfDcOntology.ttl", @"RdfDcOntology.rdf");

            ont = RDFFOAFOntology.Instance;
            GenerateOntologyFiles(ont, @"RdfFoafOntology.ttl", @"RdfFoafOntology.rdf");

            ont = RDFGEOOntology.Instance;
            GenerateOntologyFiles(ont, @"RdfGeoOntology.ttl", @"RdfGeoOntology.rdf");

            ont = RDFSIOCOntology.Instance;
            GenerateOntologyFiles(ont, @"RdfSiocOntology.ttl", @"RdfSiocOntology.rdf");

            ont = RDFSKOSOntology.Instance;
            GenerateOntologyFiles(ont, @"RdfSkosOntology.ttl", @"RdfSkosOntology.rdf");
        }

        /// <summary>
        /// https://www.w3.org/TR/skos-primer/
        /// </summary>
        static void SkosPrimer()
        {
            // The exemples comes from this documentation: https://www.w3.org/TR/skos-primer/

            // 2 SKOS Essentials
            // 2.1 Concepts

            // <http://www.example.com/animals> rdf:type skos:Concept.

            RDFOntology ont = RDFSKOSOntology.Instance;

            RDFResource conceptName = new RDFResource("http://www.example.com/");
            RDFSKOSConceptScheme conceptScheme = new RDFSKOSConceptScheme(conceptName);

            ont.Data.AddFact(conceptScheme);

            // 2.2 Labels

            /*
  ex:animals rdf:type skos:Concept;
  skos:prefLabel "animals"@en;
  skos:altLabel "creatures"@en;
  skos:prefLabel "animaux"@fr;
  skos:altLabel "créatures"@fr;
  skos:altLabel "bêtes"@fr;
  skos:hiddenLabel "betes"@fr.
             */

            RDFResource animalConceptResource = new RDFResource("http://www.example.com/animals");
            RDFSKOSConcept animalConcept = new RDFSKOSConcept(animalConceptResource);

            // 2.2.1 Preferred Lexical Labels
            RDFOntologyLiteral prefLabelLiteralEn = new RDFOntologyLiteral(new RDFPlainLiteral("animals", "en"));
            conceptScheme.AddPrefLabelAnnotation(animalConcept, prefLabelLiteralEn);

            RDFOntologyLiteral prefLabelLiteralFr = new RDFOntologyLiteral(new RDFPlainLiteral("animaux", "fr"));
            conceptScheme.AddPrefLabelAnnotation(animalConcept, prefLabelLiteralFr);

            // 2.2.2 Alternative Lexical Labels
            RDFOntologyLiteral altLabelLiteralEn = new RDFOntologyLiteral(new RDFPlainLiteral("creatures", "en"));
            conceptScheme.AddAltLabelAnnotation(animalConcept, altLabelLiteralEn);

            RDFOntologyLiteral altLabelLiteralFr = new RDFOntologyLiteral(new RDFPlainLiteral("créatures", "fr"));
            conceptScheme.AddAltLabelAnnotation(animalConcept, altLabelLiteralFr);

            RDFOntologyLiteral altLabelLiteralFr2 = new RDFOntologyLiteral(new RDFPlainLiteral("bêtes", "fr"));
            conceptScheme.AddAltLabelAnnotation(animalConcept, altLabelLiteralFr2);

            // 2.2.3 Hidden Lexical Labels
            RDFOntologyLiteral hiddenLabelLiteralFr = new RDFOntologyLiteral(new RDFPlainLiteral("betes", "fr"));
            conceptScheme.AddHiddenLabelAnnotation(animalConcept, hiddenLabelLiteralFr);


            conceptScheme.AddTopConceptRelation(animalConcept);

            #region 2.3 Semantic Relationships
            // 2.3.1 Broader/Narrower Relationships

            // The word "broader" should read here as "has broader concept"
            // the subject of a skos:broader statement is the more specific concept involved in the assertion and its object is the more generic one. 

            /*
    ex:animals rdf:type skos:Concept;
        skos:prefLabel "animals"@en;
        skos:narrower ex:mammals.
    ex:mammals rdf:type skos:Concept;
        skos:prefLabel "mammals"@en;
        skos:broader ex:animals.
             */

            RDFResource mammalsConceptResource = new RDFResource("http://www.example.com/mammals");
            RDFSKOSConcept mammalsConcept = new RDFSKOSConcept(mammalsConceptResource);

            RDFOntologyLiteral mammalsPrefLabelLiteralEn = new RDFOntologyLiteral(new RDFPlainLiteral("mammals", "en"));
            conceptScheme.AddPrefLabelAnnotation(mammalsConcept, mammalsPrefLabelLiteralEn);

            conceptScheme.AddTopConceptRelation(mammalsConcept);

            conceptScheme.AddNarrowerRelation(animalConcept, mammalsConcept);
            conceptScheme.AddBroaderRelation(mammalsConcept, animalConcept);

            // 2.3.2 Associative Relationships

            /*
ex:birds rdf:type skos:Concept;
  skos:prefLabel "birds"@en;
  skos:related ex:ornithology.
ex:ornithology rdf:type skos:Concept;
  skos:prefLabel "ornithology"@en.             
             */


            RDFResource birdsConceptResource = new RDFResource("http://www.example.com/birds");
            RDFSKOSConcept birdsConcept = new RDFSKOSConcept(birdsConceptResource);

            RDFOntologyLiteral birdsPrefLabelLiteralEn = new RDFOntologyLiteral(new RDFPlainLiteral("birds", "en"));
            conceptScheme.AddPrefLabelAnnotation(birdsConcept, birdsPrefLabelLiteralEn);

            conceptScheme.AddTopConceptRelation(birdsConcept);


            RDFResource ornithologyConceptResource = new RDFResource("http://www.example.com/ornithology");
            RDFSKOSConcept ornithologyConcept = new RDFSKOSConcept(ornithologyConceptResource);

            RDFOntologyLiteral ornithologyPrefLabelLiteralEn = new RDFOntologyLiteral(new RDFPlainLiteral("ornithology", "en"));
            conceptScheme.AddPrefLabelAnnotation(ornithologyConcept, ornithologyPrefLabelLiteralEn);

            conceptScheme.AddTopConceptRelation(ornithologyConcept);

            // the skos:related property is symmetric 
            conceptScheme.AddRelatedRelation(birdsConcept, ornithologyConcept);

            #endregion

            #region 2.4 Documentary Notes

            /*
skos:scopeNote supplies some, possibly partial, information about the intended meaning of a concept, especially as an indication of how the use of a concept is limited in indexing practice.

ex:microwaveFrequencies skos:scopeNote 
    "Used for frequencies between 1GHz to 300Ghz"@en.
            */

            RDFResource microwaveFrequenciesConceptResource = new RDFResource("http://www.example.com/microwaveFrequencies");
            RDFSKOSConcept microwaveFrequenciesConcept = new RDFSKOSConcept(microwaveFrequenciesConceptResource);

            conceptScheme.AddTopConceptRelation(microwaveFrequenciesConcept);

            conceptScheme.AddScopeNoteAnnotation(microwaveFrequenciesConcept, new RDFOntologyLiteral(new RDFPlainLiteral("Used for frequencies between 1GHz to 300Ghz", "en")));


                /*
    skos:definition supplies a complete explanation of the intended meaning of a concept.

    ex:documentation skos:definition 
        "the process of storing and retrieving information 
        in all fields of knowledge"@en.
            */

            RDFResource documentationConceptResource = new RDFResource("http://www.example.com/documentation");
            RDFSKOSConcept documentationConcept = new RDFSKOSConcept(documentationConceptResource);

            conceptScheme.AddTopConceptRelation(documentationConcept);

            conceptScheme.AddDefinitionAnnotation(documentationConcept, new RDFOntologyLiteral(new RDFPlainLiteral("the process of storing and retrieving information in all fields of knowledge", "en")));

            /*
    skos:example supplies an example of the use of a concept:

    ex:organizationsOfScienceAndCulture skos:example 
        "academies of science, general museums, world fairs"@en.
            */

            RDFResource organizationsOfScienceAndCultureConceptResource = new RDFResource("http://www.example.com/organizationsOfScienceAndCulture");
            RDFSKOSConcept organizationsOfScienceAndCultureConcept = new RDFSKOSConcept(organizationsOfScienceAndCultureConceptResource);

            conceptScheme.AddTopConceptRelation(organizationsOfScienceAndCultureConcept);

            conceptScheme.AddExampleAnnotation(organizationsOfScienceAndCultureConcept, new RDFOntologyLiteral(new RDFPlainLiteral("academies of science, general museums, world fairs", "en")));

            /*
    skos:historyNote describes significant changes to the meaning or the form of a concept:

    ex:childAbuse skos:historyNote 
        "estab. 1975; heading was: Cruelty to children [1952-1975]"@en.
                 */

            RDFResource childAbuseConceptResource = new RDFResource("http://www.example.com/childAbuse");
            RDFSKOSConcept childAbuseConcept = new RDFSKOSConcept(childAbuseConceptResource);

            conceptScheme.AddTopConceptRelation(childAbuseConcept);

            conceptScheme.AddHistoryNoteAnnotation(childAbuseConcept, new RDFOntologyLiteral(new RDFPlainLiteral("estab. 1975; heading was: Cruelty to children [1952-1975]", "en")));

            /*
             skos:editorialNote supplies information that is an aid to administrative housekeeping, such as reminders of editorial work still to be done, or warnings in the event that future editorial changes might be made:

ex:doubleclick skos:editorialNote "Review this term after company merger complete"@en.
ex:folksonomy skos:editorialNote "Check spelling with Thomas Vander Wal"@en.
            */

            RDFResource doubleclickConceptResource = new RDFResource("http://www.example.com/doubleclick");
            RDFSKOSConcept doubleclickConcept = new RDFSKOSConcept(doubleclickConceptResource);

            conceptScheme.AddTopConceptRelation(doubleclickConcept);

            conceptScheme.AddEditorialNoteAnnotation(doubleclickConcept, new RDFOntologyLiteral(new RDFPlainLiteral("Review this term after company merger complete", "en")));

            RDFResource folksonomyConceptResource = new RDFResource("http://www.example.com/folksonomy");
            RDFSKOSConcept folksonomyConcept = new RDFSKOSConcept(folksonomyConceptResource);

            conceptScheme.AddTopConceptRelation(folksonomyConcept);

            conceptScheme.AddEditorialNoteAnnotation(folksonomyConcept, new RDFOntologyLiteral(new RDFPlainLiteral("Check spelling with Thomas Vander Wal", "en")));

            /*
    skos:changeNote documents fine-grained changes to a concept, for the purposes of administration and maintenance:

    ex:tomato skos:changeNote 
      "Moved from under 'fruits' to under 'vegetables' by Horace Gray"@en.
                 */

            RDFResource tomatoConceptResource = new RDFResource("http://www.example.com/tomato");
            RDFSKOSConcept tomatoConcept = new RDFSKOSConcept(tomatoConceptResource);

            conceptScheme.AddTopConceptRelation(tomatoConcept);

            conceptScheme.AddChangeNoteAnnotation(tomatoConcept, new RDFOntologyLiteral(new RDFPlainLiteral("Moved from under 'fruits' to under 'vegetables' by Horace Gray", "en")));

            /*
             ex:pineapples rdf:type skos:Concept;
  skos:prefLabel "pineapples"@en;
  skos:prefLabel "ananas"@fr;
  skos:definition "The fruit of plants of the family Bromeliaceae"@en;
  skos:definition "Le fruit d'une plante herbacée de la famille des broméliacées"@fr.
             */

            RDFResource pineapplesConceptResource = new RDFResource("http://www.example.com/pineapples");
            RDFSKOSConcept pineapplesConcept = new RDFSKOSConcept(pineapplesConceptResource);

            conceptScheme.AddTopConceptRelation(pineapplesConcept);

            conceptScheme.AddPrefLabelAnnotation(pineapplesConcept, new RDFOntologyLiteral(new RDFPlainLiteral("pineapples", "en")));
            conceptScheme.AddPrefLabelAnnotation(pineapplesConcept, new RDFOntologyLiteral(new RDFPlainLiteral("ananas", "fr")));

            conceptScheme.AddDefinitionAnnotation(pineapplesConcept, new RDFOntologyLiteral(new RDFPlainLiteral("The fruit of plants of the family Bromeliaceae", "en")));
            conceptScheme.AddDefinitionAnnotation(pineapplesConcept, new RDFOntologyLiteral(new RDFPlainLiteral("Le fruit d'une plante herbacée de la famille des broméliacées", "fr")));

            #endregion

            #region 2.5 Concept Schemes

            /*
ex:animalThesaurus rdf:type skos:ConceptScheme;
  dct:title "Simple animal thesaurus";
  dct:creator ex:antoineIsaac.             
             */

            RDFResource animalThesaurusConceptName = new RDFResource("http://www.example.com/animalThesaurus");
            RDFSKOSConceptScheme animalThesaurusConceptScheme = new RDFSKOSConceptScheme(animalThesaurusConceptName);

            /*
ex:mammals rdf:type skos:Concept;
  skos:inScheme ex:animalThesaurus.
ex:cows rdf:type skos:Concept;
  skos:broader ex:mammals;
  skos:inScheme ex:animalThesaurus.
ex:fish rdf:type skos:Concept;
  skos:inScheme ex:animalThesaurus.             
             */

            RDFSKOSConcept mammalsConcept2 = new RDFSKOSConcept(new RDFResource("http://www.example.com/mammals"));
            RDFSKOSConcept cowsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/cows"));
            RDFSKOSConcept fishConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/fish"));

            //animalThesaurusConceptScheme.AddConcept(mammalsConcept2);
            animalThesaurusConceptScheme.AddConcept(cowsConcept);
            //animalThesaurusConceptScheme.AddConcept(fishConcept);

            animalThesaurusConceptScheme.AddTopConceptRelation(mammalsConcept2);
            animalThesaurusConceptScheme.AddTopConceptRelation(fishConcept);

            animalThesaurusConceptScheme.AddBroaderRelation(cowsConcept, mammalsConcept2);

            var graphAnimalThesaurus = animalThesaurusConceptScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            graphAnimalThesaurus.ToFile(RDFModelEnums.RDFFormats.Turtle, @"animalThesaurus.ttl");

            graphAnimalThesaurus.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"animalThesaurus.rdf");

            /*
ex:animalThesaurus rdf:type skos:ConceptScheme;
  skos:hasTopConcept ex:mammals;
  skos:hasTopConcept ex:fish.             
             */

            #endregion

            #region 3 Networking Knowledge Organization Systems on the Semantic Web

            // 3.1 Mapping Concept Schemes

            /*
ex1:referenceAnimalScheme rdf:type skos:ConceptScheme;
   dct:title "Extensive list of animals"@en. 
ex1:animal rdf:type skos:Concept;
   skos:prefLabel "animal"@en;
   skos:inScheme ex1:referenceAnimalScheme.
ex1:platypus rdf:type skos:Concept;
   skos:prefLabel "platypus"@en;
   skos:inScheme ex1:referenceAnimalScheme.

ex2:eggSellerScheme rdf:type skos:ConceptScheme;
   dct:title "Obsessed egg-seller's vocabulary"@en. 
ex2:eggLayingAnimals rdf:type skos:Concept;
   skos:prefLabel "animals that lay eggs"@en;
   skos:inScheme ex2:eggSellerScheme.
ex2:animals rdf:type skos:Concept;
   skos:prefLabel "animals"@en;
   skos:inScheme ex2:eggSellerScheme.
ex2:eggs rdf:type skos:Concept;
   skos:prefLabel "eggs"@en;
   skos:inScheme ex2:eggSellerScheme.      
   

ex1:platypus skos:broadMatch ex2:eggLayingAnimals.
ex1:platypus skos:relatedMatch ex2:eggs.
ex1:animal skos:exactMatch ex2:animals.
             */

            RDFSKOSConceptScheme referenceAnimalScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/1/referenceAnimalScheme"));

            RDFSKOSConcept animalConcept2 = new RDFSKOSConcept(new RDFResource("http://www.example.com/1/animal"));
            RDFSKOSConcept platypusConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/1/platypus"));

            referenceAnimalScheme.AddConcept(animalConcept2);
            referenceAnimalScheme.AddConcept(platypusConcept);

            RDFSKOSConceptScheme eggSellerScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/2/eggSellerScheme"));

            RDFSKOSConcept eggLayingAnimalsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/2/eggLayingAnimals"));
            RDFSKOSConcept animalsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/2/animals"));
            RDFSKOSConcept eggsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/2/eggs"));

            eggSellerScheme.AddConcept(eggLayingAnimalsConcept);
            eggSellerScheme.AddConcept(animalsConcept);
            eggSellerScheme.AddConcept(eggsConcept);


            //var tonyOnt = new RDFOntology(new RDFResource("http://www.example.com/tony"));
            //tonyOnt.Data.AddFact(referenceAnimalScheme);
            //tonyOnt.Data.AddFact(eggSellerScheme);
            //var tonyGraph = tonyOnt.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);

            // ex1:platypus skos:broadMatch ex2:eggLayingAnimals.
            referenceAnimalScheme.AddBroadMatchRelation(platypusConcept, eggLayingAnimalsConcept);
            // ex1: platypus skos:relatedMatch ex2:eggs.
            referenceAnimalScheme.AddRelatedMatchAssertion(platypusConcept, eggsConcept);
            // ex1:animal skos:exactMatch ex2:animals.
            referenceAnimalScheme.AddExactMatchRelation(animalConcept2, animalsConcept);


            var g1 = referenceAnimalScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            var g2 = eggSellerScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);

            g1.ToFile(RDFModelEnums.RDFFormats.Turtle, @"g1.ttl");
            g2.ToFile(RDFModelEnums.RDFFormats.Turtle, @"g2.ttl");

            RDFGraph mappingGraph = new RDFGraph();
            mappingGraph = mappingGraph.UnionWith(g1);
            mappingGraph = mappingGraph.UnionWith(g2);

            //mappingGraph.

            // Write the graph to disk as a turtle file
            mappingGraph.ToFile(RDFModelEnums.RDFFormats.Turtle, @"Mapping.ttl");

            mappingGraph.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"Mapping.rdf");

            #endregion

            // 4 Advanced SKOS: When KOSs are not Simple Anymore
            AdvancedSKOS();


            //var tony = new RDFOntologyFact(new RDFResource("http://www.example.com/animals"));

            /*
            // CREATE ONTOLOGY LITERALS
            var markName = new RDFOntologyLiteral(new RDFPlainLiteral("Mark", "en-US"));
            var tonyName = new RDFOntologyLiteral(new RDFPlainLiteral("Tony", "en-US"));

            // ADD ONTOLOGY FACTS TO ONTOLOGY DATA
            ont.Data.AddFact(mark);
            ont.Data.AddFact(tony);

            // ADD TYPE RELATIONS TO ONTOLOGY DATA
            ont.Data.AddClassTypeRelation(mark, human);
            ont.Data.AddClassTypeRelation(tony, human);

            // ADD ASSERTIONS TO ONTOLOGY DATA
            ont.Data.AddAssertionRelation(mark, hasPet, garfield);
            */


            var g = conceptScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"test2.ttl");

            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"test2.rdf");

            // Create a graph from the ontology
            var graph = ont.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            graph.SetContext(new Uri("http://www.example.com/"));

            // Write the graph to disk as a turtle file
            graph.ToFile(RDFModelEnums.RDFFormats.Turtle, @"test.ttl");

            graph.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"test.rdf");

        }

        /// <summary>
        /// 4 Advanced SKOS: When KOSs are not Simple Anymore
        /// </summary>
        private static void AdvancedSKOS()
        {
            // 4.1 Collections of Concepts

            // Labeled Collections
            LabeledCollections();

            // Ordered Collections
            OrderedCollections();

            // 4.2 Advanced Documentation Features
            AdvancedDocumentation();

            // 4.3 Relationships between Labels
            RelationshipsBetweenLabels();

            // 4.4 Coordinating Concepts 

            // 4.5 Transitive Hierarchies

            // 4.6 Notations
            Notations();


        }

        private static void LabeledCollections()
        {
            /*
ex:milk rdf:type skos:Concept;
  skos:prefLabel "milk"@en.
ex:cowMilk rdf:type skos:Concept; 
  skos:prefLabel "cow milk"@en;
  skos:broader ex:milk.
ex:goatMilk rdf:type skos:Concept; 
  skos:prefLabel "goat milk"@en;
  skos:broader ex:milk.
ex:buffaloMilk rdf:type skos:Concept; 
  skos:prefLabel "buffalo milk"@en;
  skos:broader ex:milk.

_:b0 rdf:type skos:Collection;
   skos:prefLabel "milk by source animal"@en;
   skos:member ex:cowMilk;
   skos:member ex:goatMilk;
   skos:member ex:buffaloMilk.             
             */

            RDFSKOSConceptScheme advancedScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/LabeledCollections"));

            RDFSKOSConcept milkConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/milk"));
            RDFSKOSConcept cowMilkConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/cowMilk"));
            RDFSKOSConcept goatMilkConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/goatMilk"));
            RDFSKOSConcept buffaloMilkConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/buffaloMilk"));

            advancedScheme.AddConcept(milkConcept);
            advancedScheme.AddConcept(cowMilkConcept);
            advancedScheme.AddConcept(goatMilkConcept);
            advancedScheme.AddConcept(buffaloMilkConcept);

            advancedScheme.AddPrefLabelAnnotation(milkConcept, new RDFOntologyLiteral(new RDFPlainLiteral("milk", "en")));
            advancedScheme.AddPrefLabelAnnotation(cowMilkConcept, new RDFOntologyLiteral(new RDFPlainLiteral("cow milk", "en")));
            advancedScheme.AddPrefLabelAnnotation(goatMilkConcept, new RDFOntologyLiteral(new RDFPlainLiteral("goat milk", "en")));
            advancedScheme.AddPrefLabelAnnotation(buffaloMilkConcept, new RDFOntologyLiteral(new RDFPlainLiteral("buffalo milk", "en")));

            // Note that, according to the SKOS data model, collections are disjoint from concepts. It is therefore impossible to use SKOS semantic relations (see Section 2.3) 
            // to have a collection directly fit into a SKOS semantic network. In other words, grouping concepts into collections does not replace assertions about the concepts' place in a concept scheme. 
            // For instance, in the above "milk" example, all source-defined milks must be explicitly attached to a more generic ex:milk using the skos:broader property:
            advancedScheme.AddBroaderRelation(cowMilkConcept, milkConcept);
            advancedScheme.AddBroaderRelation(goatMilkConcept, milkConcept);
            advancedScheme.AddBroaderRelation(buffaloMilkConcept, milkConcept);

            RDFSKOSCollection collection = new RDFSKOSCollection(new RDFResource("_:b0"));

            advancedScheme.AddCollection(collection);

            //advancedScheme.AddPrefLabelAnnotation(collection, new RDFOntologyLiteral(new RDFPlainLiteral("milk by source animal", "en")));

            collection.AddConcept(cowMilkConcept);
            collection.AddConcept(goatMilkConcept);
            collection.AddConcept(buffaloMilkConcept);

            var g = advancedScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"LabeledCollections.ttl");
            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"LabeledCollections.rdf");
        }

        private static void OrderedCollections()
        {
            /*
ex:infants rdf:type skos:Concept; 
  skos:prefLabel "infants"@en.
ex:children rdf:type skos:Concept; 
  skos:prefLabel "children"@en.
ex:adults rdf:type skos:Concept; 
  skos:prefLabel "adults"@en.

_:b0 rdf:type skos:OrderedCollection;
   skos:prefLabel "people by age"@en;
   skos:memberList _:b1.
_:b1 rdf:first ex:infants;
   rdf:rest _:b2.
_:b2 rdf:first ex:children;
   rdf:rest _:b3.
_:b3 rdf:first ex:adults;
   rdf:rest rdf:nil.
             */

            RDFSKOSConceptScheme advancedScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/orderedCollectionsScheme"));

            RDFSKOSConcept infantsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/infants"));
            RDFSKOSConcept childrenConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/children"));
            RDFSKOSConcept adultsConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/adults"));

            advancedScheme.AddConcept(infantsConcept);
            advancedScheme.AddConcept(childrenConcept);
            advancedScheme.AddConcept(adultsConcept);

            advancedScheme.AddPrefLabelAnnotation(infantsConcept, new RDFOntologyLiteral(new RDFPlainLiteral("infants", "en")));
            advancedScheme.AddPrefLabelAnnotation(childrenConcept, new RDFOntologyLiteral(new RDFPlainLiteral("children", "en")));
            advancedScheme.AddPrefLabelAnnotation(adultsConcept, new RDFOntologyLiteral(new RDFPlainLiteral("adults", "en")));

            RDFSKOSOrderedCollection collection = new RDFSKOSOrderedCollection(new RDFResource("_:b0"));

            advancedScheme.AddOrderedCollection(collection);

            //advancedScheme.AddPrefLabelAnnotation(collection, new RDFOntologyLiteral(new RDFPlainLiteral("milk by source animal", "en")));

            collection.AddConcept(infantsConcept);
            collection.AddConcept(childrenConcept);
            collection.AddConcept(adultsConcept);

            var g = advancedScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"orderedCollectionsScheme.ttl");
            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"orderedCollectionsScheme.rdf");
        }

        private static void AdvancedDocumentation()
        {
            /*
Documentation as an RDF literal

Here, documentation statements have simple RDF literals as objects, as illustrated by all examples of Section 2.4. This is the simplest way to document concepts, and it is expected to fit most common applications.
            */

            /*
Documentation as a Related Resource Description
            
In this second pattern, the object of a documentation statement consists of a general non-literal RDF node—that is, a resource node (possibly blank) that can be the subject of further RDF statements [RDF-PRIMER]. This is especially useful to represent with RDF more information about the documentation itself, such as its creator or creation date. This is typically done using the RDF rdf:value utility property, as in the following example, which uses a blank node:

ex:tomato skos:changeNote [
  rdf:value "Moved from under 'fruits' to under 'vegetables'"@en;
  dct:creator ex:HoraceGray; 
  dct:date "1999-01-23" 
].
ex:HoraceGray rdf:type foaf:Person; foaf:name "Horace Gray".
            */

            RDFSKOSConceptScheme advancedScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/advancedDocumentationScheme"));

            RDFSKOSConcept tomatoConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/tomato"));
            advancedScheme.AddConcept(tomatoConcept);
            advancedScheme.AddPrefLabelAnnotation(tomatoConcept, new RDFOntologyLiteral(new RDFPlainLiteral("tomato", "en")));

            RDFResource resource = new RDFResource("_:b0");
            //RDFOntologyAnnotationProperty annotationProperty = resource.ToRDFOntologyAnnotationProperty();

            RDFSKOSConcept zoologyConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/zoology"));
            advancedScheme.AddConcept(zoologyConcept);
            advancedScheme.AddPrefLabelAnnotation(zoologyConcept, new RDFOntologyLiteral(new RDFPlainLiteral("zoology", "en")));


            //advancedScheme.AddChangeNoteAnnotation(tomatoConcept, annotationProperty);

            //var taxonomyEntry = new RDFOntologyTaxonomyEntry(tomatoConcept, RDFVocabulary.SKOS.CHANGE_NOTE.ToRDFOntologyAnnotationProperty(), annotationProperty)
            //advancedScheme.Annotations.ChangeNote.AddEntry(taxonomyEntry);


            var g = advancedScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            g.AddTriple(new RDFTriple(new RDFResource("ex:tomato"), new RDFResource("http://www.w3.org/2004/02/skos/core#changeNote"), resource));

            g.AddTriple(new RDFTriple(resource, new RDFResource("http://www.w3.org/1999/02/22-rdf-syntax-ns#value"), new RDFPlainLiteral("Moved from under 'fruits' to under 'vegetables'", "en")));
            g.AddTriple(new RDFTriple(resource, new RDFResource("http://purl.org/dc/terms/creator"), new RDFResource("ex:HoraceGray")));
            g.AddTriple(new RDFTriple(resource, new RDFResource("http://purl.org/dc/terms/date"), new RDFPlainLiteral("1999-01-23")));

            g.AddTriple(new RDFTriple(new RDFResource("ex:HoraceGray"), new RDFResource("http://www.w3.org/1999/02/22-rdf-syntax-ns#type"), new RDFResource("foaf:Person")));
            g.AddTriple(new RDFTriple(new RDFResource("ex:HoraceGray"), new RDFResource("http://xmlns.com/foaf/0.1/name"), new RDFPlainLiteral("Horace Gray")));


            /*
Documentation as a Document Reference

A third option consists of introducing, as the object of a documentation statement, the URI of a document, for instance a Web page. Note that this pattern, closely related to the previous one, also allows the definition of further metadata for this document using RDF:

ex:zoology skos:definition ex:zoology.txt.
ex:zoology.txt dct:creator ex:JohnSmith.             
             */


            g.AddTriple(new RDFTriple(new RDFResource("ex:zoology"), new RDFResource("http://www.w3.org/2004/02/skos/core#definition"), new RDFResource("ex:zoology.txt")));
            g.AddTriple(new RDFTriple(new RDFResource("ex:zoology.txt"), new RDFResource("http://purl.org/dc/terms/creator"), new RDFResource("ex:JohnSmith")));


            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"advancedDocumentationScheme.ttl");
            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"advancedDocumentationScheme.rdf");
        }

        private static void RelationshipsBetweenLabels()
        {
            RDFSKOSConceptScheme advancedScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/relationshipsBetweenLabelsScheme"));

            /*
ex:FAOlabel1 rdf:type skosxl:Label;
  skosxl:literalForm "Food and Agriculture Organization"@en.
ex:FAOlabel2 rdf:type skosxl:Label;
  skosxl:literalForm "FAO"@en.
            */

            RDFSKOSLabel faoLabel1 = new RDFSKOSLabel(new RDFResource("ex:FAOlabel1"));
            //advancedScheme.AddLiteralFormAssertion(faoLabel1, new RDFOntologyLiteral(new RDFPlainLiteral("Food and Agriculture Organization", "en")));

            RDFSKOSLabel faoLabel2 = new RDFSKOSLabel(new RDFResource("ex:FAOlabel2"));
            //advancedScheme.AddLiteralFormAssertion(faoLabel2, new RDFOntologyLiteral(new RDFPlainLiteral("FAO", "en")));


            /*
skosxl:Label instances can then be related to concepts using properties (skosxl:prefLabel, skosxl:altLabel, skosxl:hiddenLabel) that mirror the standard literal-based labeling constructs. Finally, these instances can be linked together by skosxl:labelRelation statements:
            
ex:FAO rdf:type skos:Concept;
  skosxl:prefLabel ex:FAOlabel1; 
  skosxl:altLabel ex:FAOlabel2.
ex:FAOlabel2 skosxl:labelRelation ex:FAOlabel1.
            */

            RDFSKOSConcept faoConcept = new RDFSKOSConcept(new RDFResource("http://www.example.com/FAO"));
            advancedScheme.AddConcept(faoConcept);

            // Bug si on cherche à en mettre deux

            //advancedScheme.AddPrefLabelRelation(faoConcept, faoLabel1, new RDFOntologyLiteral(new RDFPlainLiteral("Food and Agriculture Organization", "en")));
            advancedScheme.AddPrefLabelRelation(faoConcept, faoLabel1, new RDFOntologyLiteral(new RDFPlainLiteral("Organisation des Nations unies pour l'alimentation et l'agriculture", "fr")));
            //advancedScheme.AddAltLabelRelation(faoConcept, faoLabel2, new RDFOntologyLiteral(new RDFPlainLiteral("FAO", "en")));
            advancedScheme.AddAltLabelRelation(faoConcept, faoLabel2, new RDFOntologyLiteral(new RDFPlainLiteral("ONUAA", "fr")));

            advancedScheme.AddLabelRelation(faoLabel2, faoLabel1);



            var g = advancedScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            /*
Such a solution is however not complete: an "acronym-sensitive" application would miss the actual information that the two labels are indeed in an acronymy relationship. 
Such an application would also miss the direction of the link. 
SKOS-XL users are therefore encouraged to specialize skosxl:labelRelation so as to fit their application-specific requirements, as in the following:

ex:isAcronymOf rdfs:subPropertyOf skosxl:labelRelation.
ex:FAOlabel2 ex:isAcronymOf ex:FAOlabel1.
 */

            g.AddTriple(new RDFTriple(new RDFResource("ex:isAcronymOf"), new RDFResource(RDFVocabulary.RDFS.SUB_PROPERTY_OF.ToString()), new RDFResource(RDFVocabulary.SKOS.SKOSXL.LABEL_RELATION.ToString())));
            g.AddTriple(new RDFTriple(new RDFResource("ex:FAOlabel2"), new RDFResource("ex:isAcronymOf"), new RDFResource("ex:FAOlabel1")));


            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"relationshipsBetweenLabelsScheme.ttl");
            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"relationshipsBetweenLabelsScheme.rdf");
        }

        private static void Notations()
        {
            /*
ex:udc512 skos:prefLabel "Algebra"@en ;
  skos:notation "512"^^ex:UDCNotation .              
             */

            RDFSKOSConceptScheme advancedScheme = new RDFSKOSConceptScheme(new RDFResource("http://www.example.com/notationsScheme"));

            RDFSKOSConcept udc512Concept = new RDFSKOSConcept(new RDFResource("http://www.example.com/udc512"));
            advancedScheme.AddConcept(udc512Concept);

            advancedScheme.AddPrefLabelAnnotation(udc512Concept, new RDFOntologyLiteral(new RDFPlainLiteral("Algebra", "en")));

            // BUG : on ne peut choisir son type en dehors d'un enum "512"^^ex:UDCNotation .   
            advancedScheme.AddNotationRelation(udc512Concept, new RDFOntologyLiteral(new RDFTypedLiteral("512", RDFModelEnums.RDFDatatypes.XSD_STRING)));

            

            var g = advancedScheme.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);
            g.SetContext(new Uri("http://www.example.com/"));

            g.ToFile(RDFModelEnums.RDFFormats.Turtle, @"notationsScheme.ttl");
            g.ToFile(RDFModelEnums.RDFFormats.RdfXml, @"notationsScheme.rdf");

            /*
ex:udc512 skos:prefLabel "Algebra"@en ;
  skos:notation "512"^^ex:UDCNotation ;
  skos:prefLabel "512" .             
             */

        }


        private static void GenerateOntologyFiles(RDFOntology ont, string turtleFile, string rdfXmlFile)
        {
            ValidateOntology(ont);

            // ONTOLOGY: REASONER
            ReasonOnOntology(ref ont);

            // Create a graph from the ontology
            var graph = ont.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData);

            // Write the graph to disk as a turtle file
            graph.ToFile(RDFModelEnums.RDFFormats.Turtle, turtleFile);

            graph.ToFile(RDFModelEnums.RDFFormats.RdfXml, rdfXmlFile);
        }


        private static void ValidateOntology(RDFOntology ont)
        {
            // VALIDATE ONTOLOGY
            RDFOntologyValidatorReport report = ont.Validate();

            // READ VALIDATOR REPORT: WARNINGS
            List<RDFOntologyValidatorEvidence> warnings = report.SelectWarnings();
            foreach (RDFOntologyValidatorEvidence warning in warnings)
            {
                Console.WriteLine("Validation Rule:" + warning.EvidenceProvenance);
                Console.WriteLine("Validation Message:" + warning.EvidenceMessage);
                Console.WriteLine("Validation Suggestion:" + warning.EvidenceSuggestion);
            }
            
            // READ VALIDATOR REPORT: ERRORS
            List<RDFOntologyValidatorEvidence> errors = report.SelectErrors();
            foreach (RDFOntologyValidatorEvidence error in errors)
            {
                Console.WriteLine("Validation Rule:" + error.EvidenceProvenance);
                Console.WriteLine("Validation Message:" + error.EvidenceMessage);
                Console.WriteLine("Validation Suggestion:" + error.EvidenceSuggestion);
            }
        }

        private static void ReasonOnOntology(ref RDFOntology ont)
        {
            // CREATE A REASONER AND APPLY IT ON ONTOLOGY
            var rep = RDFOntologyReasoner
            .CreateNew()
            .WithRule(RDFOntologyReasonerRuleset.SubClassTransitivity)
            .WithRule(RDFOntologyReasonerRuleset.SubPropertyTransitivity)
            .WithRule(RDFOntologyReasonerRuleset.DomainEntailment)
            .WithRule(RDFOntologyReasonerRuleset.RangeEntailment)
            .WithRule(RDFOntologyReasonerRuleset.ClassTypeEntailment)
            .WithRule(RDFOntologyReasonerRuleset.PropertyEntailment)
            .WithRule(RDFOntologyReasonerRuleset.DifferentFromEntailment)
            .WithRule(RDFOntologyReasonerRuleset.DisjointWithEntailment)
            .WithRule(RDFOntologyReasonerRuleset.EquivalentClassTransitivity)
            .WithRule(RDFOntologyReasonerRuleset.EquivalentPropertyTransitivity)
            .WithRule(RDFOntologyReasonerRuleset.InverseOfEntailment)
            .WithRule(RDFOntologyReasonerRuleset.SameAsEntailment)
            .WithRule(RDFOntologyReasonerRuleset.SameAsTransitivity)
            .WithRule(RDFOntologyReasonerRuleset.SymmetricPropertyEntailment)
            .WithRule(RDFOntologyReasonerRuleset.TransitivePropertyEntailment)
            .ApplyToOntology(ref ont); //ontology must be passed by reference
                                       // ITERATE OVER THE MATERIALIZED INFERENCES
            foreach (var ev in rep)
            {
                Console.WriteLine(ev.EvidenceProvenance + ";" + ev.EvidenceCategory + ";" + ev.EvidenceContent);
            }
        }

        private static void QueryOntology(RDFOntology ont)
        {
            /*
             * Build the SPARQL query
             * 
SELECT ?F
WHERE {
  {
    ?C <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://www.w3.org/2002/07/owl#Class> .
    ?F <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> ?C .
    ?F <http://www.w3.org/2000/01/rdf-schema#label> "violet" .
  }
}
             */

            // BUILD A SPARQL SELECT QUERY
            RDFVariable f = new RDFVariable("f"); RDFVariable c = new RDFVariable("c");
            RDFSelectQuery q = new RDFSelectQuery()
            .AddPatternGroup(new RDFPatternGroup("FACTS_WITH_VIOLET_LABEL")
            .AddPattern(new RDFPattern(c, RDFVocabulary.RDF.TYPE, RDFVocabulary.OWL.CLASS))
            .AddPattern(new RDFPattern(f, RDFVocabulary.RDF.TYPE, c))
            .AddPattern(new RDFPattern(f, RDFVocabulary.RDFS.LABEL, new RDFPlainLiteral("violet"))))
            .AddProjectionVariable(f);

            // APPLY IT ON ONTOLOGY

            RDFSelectQueryResult selectResult = q.ApplyToGraph(ont.ToRDFGraph(RDFSemanticsEnums.RDFOntologyInferenceExportBehavior.ModelAndData));
            selectResult.ToSparqlXmlResult(@"result.rdf");
        }

    }
}
