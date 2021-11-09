more_arguments:
	TOKEN_KIND_OP_COMMA expression

arguments:
	expression more_arguments*

expression:
	primary_expression
	expression TOKEN_KIND_OP_PLUS   expression
	expression TOKEN_KIND_OP_MINUS  expression
	expression TOKEN_KIND_OP_STAR   expression
	expression TOKEN_KIND_OP_SLASH  expression

primary_expression:
	TOKEN_KIND_LITERAL_INTEGER
	TOKEN_KIND_LITERAL_DECIMAL
	TOKEN_KIND_LITERAL_SSTRING
	TOKEN_KIND_LITERAL_DSTRING
	TOKEN_KIND_LITERAL_BSTRING
	TOKEN_KIND_WORD







