# Datatablejs with server side data

# database setup
	execute sql script file first from .\dbScript.sql. There are two sample tables and one store procedure which is responsible for create dynamic queries.

# web.config
	Change connection string in web.config file(set your proper connection string).

	Now you can start project from visual studio hitting F5 key from your keyboard.
	
	
# How to use backend?

1) you have to configure your params in .\Models\GridData.cs file:

	-Following params are there in that file in each case:
	
		1) type= to identify the query
		2) ColumnsName= columns name (* is also allowed)
		3) PageNumber=page number(int)
		4) RecordPerPage= how many records you want to get per page from sql server
		5) SortColumn = by default which columns has been sort while user load this grid
		6) SortOrder = by default which orders has been set to default sort column.(values are like 'asc' or 'desc')
		7) TableName = name of your table.(suppose you have two table with join then set TableName=' tbl1 A INNER JOIN tbl2 B ON B.col1=A.col1 LEFT JOIN tbl3 C ON C.col1=A.col2')
		8) WhereClause= if you have fixed whereclause while load the grid then set here.(ex. WhereClause='IsActive=1',etc)
		9) ExportedFileName= filename of exported. means if user click on pdf icon then this named file will be generated.
		10) ExportedColumns= these are the columns which are available on exported file. if you want to hide any column then write [Hidden] after column name.
		

# How to use jquery wrapper(client side jquery function)
	Default params = { Url: '/Home/GetGridData', PagerInfo: true, SearchParams: {}, RecordPerPage: 10, DataType: 'POST', Columns: [], Mode: '', FixClause: '', SortColumn: '0', SortOrder: 'asc', ExportIcon: true, ColumnSelection: true, IsAddShow: true, OnAdd: fnAdd, GrdLabels: JSON.stringify({ Show: "Showing", To: "to", Of: "of", Entries: "entries", Search: "Search", First: "first", Last: "last", Next: "next", Previous: "previous", SortAsc: "activate to sort column ascending", SortDesc: "activate to sort column descending", Add: "Add", ExportTo: "Export To ", Excel: "Excel", Pdf: "Pdf", Csv: "Csv", Word: "Word" }), DrawCallback: fn_drawCallback };
      -Url: url of ajax call
	  -Pagerinfo: paging enable or not
	  -SearchParams: search para if you want to pass from ajax call
	  -RecordPerPage: no of rows per page
	  -DataType: ajax call type (post or get)
	  -Column: array of columns
	  -Mode: to identify the query
	  -FixClause: fixed where clause and if it's not affects your critical data then you can pass here, other wise you can set from backend.(because this things appearing in inspect element of any browser)
	  -SortColumn: default sort column index(you need to pass index here,not column name)
	  -SortOrder: default sort order(asc or desc)
	  -ExportIcon: export icon should be display or not.
	  -ColumnSelection: column selection enable/disable
	  -IsAddShow: add button on top most right corner will be display
	  - OnAdd: the function which is called when user click on add button.
	  -GrdLabels: labels of grid plugin.(it's used when multiple languages are there on your page)
	  -DrawCallback: function which is called every draw event.