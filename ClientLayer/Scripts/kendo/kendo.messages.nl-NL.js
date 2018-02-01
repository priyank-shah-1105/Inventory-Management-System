
kendo.ui.Locale = "Nederlands NL (nl-NL)";

var conflictError = "This record is linked with other record and cannot be deleted.";
var processError = "Er is een fout opgetreden tijdens het verwerken van uw opdracht.";
var norecords = "Geen gegevens.";

kendo.ui.ColumnMenu.prototype.options.messages =
  $.extend(kendo.ui.ColumnMenu.prototype.options.messages, {

      /* COLUMN MENU MESSAGES 
       ****************************************************************************/
      sortAscending: "Sorteer Oplopend",
      sortDescending: "Sorteer Aflopend",
      filter: "Filteren",
      columns: "Kolommen"
      /***************************************************************************/
  });

kendo.ui.Groupable.prototype.options.messages =
  $.extend(kendo.ui.Groupable.prototype.options.messages, {

      /* GRID GROUP PANEL MESSAGES 
       ****************************************************************************/
      empty: "Sleep hier een kolomnaam om te groeperen op deze kolom"
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.messages =
  $.extend(kendo.ui.FilterMenu.prototype.options.messages, {

      /* FILTER MENU MESSAGES 
       ***************************************************************************/
      info: "Laat items zien met een waarde die:", // sets the text on top of the filter menu
      isTrue: "juist is",                   // sets the text for "isTrue" radio button
      isFalse: "fout is",                 // sets the text for "isFalse" radio button
      filter: "Filteren",                    // sets the text for the "Filter" button
      clear: "Leegmaken",                      // sets the text for the "Clear" button
      and: "En",
      or: "Of",
      selectValue: "-Selecteer een waarde-"
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.operators =
  $.extend(kendo.ui.FilterMenu.prototype.options.operators, {

      /* FILTER MENU OPERATORS (for each supported data type) 
       ****************************************************************************/
      string: {
          eq: "Gelijk is aan",
          neq: "Niet gelijk is aan",
          startswith: "Start met",
          contains: "Bevat",
          doesnotcontain: "Niet bevat",
          endswith: "Eindigd op"
      },
      number: {
          eq: "Gelijk is aan",
          neq: "Niet gelijk is aan",
          gte: "Groter dan of gelijk is aan",
          gt: "Groter is dan",
          lte: "Kleiner dan of gelijk is aan",
          lt: "Kleiner is dan"
      },
      date: {
          eq: "Gelijk is aan",
          neq: "Niet gelijk is aan",
          gte: "Later of gelijk is aan",
          gt: "Later is dan",
          lte: "Vroeger of gelijk is aan",
          lt: "Vroeger is dan"
      },
      enums: {
          eq: "Gelijk is aan",
          neq: "Niet gelijk is aan"
      }
      /***************************************************************************/
  });

kendo.ui.Pager.prototype.options.messages =
  $.extend(kendo.ui.Pager.prototype.options.messages, {

      /* PAGER MESSAGES 
       ****************************************************************************/
      display: "{0} - {1} van {2} items",
      empty: "Geen items om weer te geven",
      page: "Pagina",
      of: "van {0}",
      itemsPerPage: "items per pagina",
      first: "Ga naar de eerste pagina",
      previous: "Ga naar de vorige pagina",
      next: "Ga naar de volgende pagina",
      last: "Ga naar de laatste pagina",
      refresh: "Vernieuwen"
      /***************************************************************************/
  });

kendo.ui.Validator.prototype.options.messages =
  $.extend(kendo.ui.Validator.prototype.options.messages, {

      /* VALIDATOR MESSAGES 
       ****************************************************************************/
      required: "{0} is verplicht",
      pattern: "{0} is ongeldig",
      min: "{0} moet groter zijn of gelijk aan {1}",
      max: "{0} moet kleiner zijn of gelijk aan {1}",
      step: "{0} is ongeldig",
      email: "{0} is een ongeldig email adres",
      url: "{0} is een ongeldige URL",
      date: "{0} is een foutieve datum"
      /***************************************************************************/
  });

kendo.ui.ImageBrowser.prototype.options.messages =
  $.extend(kendo.ui.ImageBrowser.prototype.options.messages, {

      /* IMAGE BROWSER MESSAGES 
       ****************************************************************************/
      uploadFile: "Uploaden",
      orderBy: "Sorteren op",
      orderByName: "Naam",
      orderBySize: "Grootte",
      directoryNotFound: "Kon geen folder vinden met deze naam.",
      emptyFolder: "Lege folder",
      deleteFile: 'Weet u zeker dat u "{0}" wenst te verwijderen?',
      invalidFileType: "Het geselecteerde bestand \"{0}\" is ongeldig. Geldige bestandstypes zijn {1}.",
      overwriteFile: "Een bestand met naam \"{0}\" bestaat reeds in de huidge folder. Wilt u deze overschrijven?",
      dropFilesHere: "Sleep hier een bestand om te uploaden"
      /***************************************************************************/
  });

kendo.ui.Editor.prototype.options.messages =
  $.extend(kendo.ui.Editor.prototype.options.messages, {

      /* EDITOR MESSAGES 
       ****************************************************************************/
      bold: "Vet",
      italic: "Cursief",
      underline: "Onderlijnen",
      strikethrough: "Doorstrepen",
      superscript: "Superscript",
      subscript: "Subscript",
      justifyCenter: "Centreren",
      justifyLeft: "Links uitlijnen",
      justifyRight: "Rechts uitlijnen",
      justifyFull: "Uitlijnen",
      insertUnorderedList: "Ongeordende lijst toevoegen",
      insertOrderedList: "Geordende lijst toevoegen",
      indent: "Inspringen",
      outdent: "Uitspringen",
      createLink: "Hyperlink toevoegen",
      unlink: "Hyperlink verwijderen",
      insertImage: "Afbeelding toevoegen",
      insertHtml: "HTML toevoegen",
      fontName: "Selecteer een lettertype",
      fontNameInherit: "(basis lettertype)",
      fontSize: "Selecteer grootte van lettertype",
      fontSizeInherit: "(basis grootte)",
      formatBlock: "Structuur",
      foreColor: "Kleur",
      backColor: "Achtergrondkleur",
      style: "Stijlen",
      emptyFolder: "Lege Folder",
      uploadFile: "Uploaden",
      orderBy: "Sorteren op:",
      orderBySize: "Grootte",
      orderByName: "Naam",
      invalidFileType: "Het geselecteerde bestand \"{0}\" is ongeldig. Geldige bestandtypes zijn {1}.",
      deleteFile: 'Weet u zeker dat u "{0}" wilt verwijderen?',
      overwriteFile: 'Een bestand met naam \"{0}\" bestaat reeds in de huidge folder. Wilt u deze overschrijven?',
      directoryNotFound: "Kon geen folder vinden met deze naam.",
      imageWebAddress: "Web adres",
      imageAltText: "Alternatieve tekst",
      dialogInsert: "Toevoegen",
      dialogButtonSeparator: "of",
      dialogCancel: "Annuleren"
      /***************************************************************************/
  });