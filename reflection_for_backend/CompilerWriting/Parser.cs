using Collections = System.Collections.Generic;
using Text = System.Text;

public sealed class Parser
{
	private int index;
	private Collections.IList<object> tokens;
	private readonly Stmt result;

	public Parser(Collections.IList<object> tokens)
	{
		this.tokens = tokens;
		this.index = 0;
		this.result = this.ParseStmt();
		
		if (this.index != this.tokens.Count)
			throw new System.Exception("expected EOF");
	}

	public Stmt Result
	{
		get { return result; }
	}

    private Stmt ParseStmt()
    {
        Stmt result;

		if (this.index == this.tokens.Count)
		{
			throw new System.Exception("expected statement, got EOF");
		}

        // <stmt> := print <expr> 

        // <expr> := <string>
	    // | <int>
	    // | <arith_expr>
	    // | <ident>
		if (this.tokens[this.index].Equals("print"))
		{
			this.index++;
			Print print = new Print();
			print.Expr = this.ParseExpr();
			result = print;
		}
		else if (this.tokens[this.index].Equals("var"))
		{
			this.index++;
			DeclareVar declareVar = new DeclareVar();

			if (this.index < this.tokens.Count &&
				this.tokens[this.index] is string)
			{
				declareVar.Ident = (string)this.tokens[this.index];
			}
			else
			{
				throw new System.Exception("expected variable name after 'var'");
			}

			this.index++;

			if (this.index == this.tokens.Count ||
				this.tokens[this.index] != Scanner.Equal)
			{
				throw new System.Exception("expected = after 'var ident'");
			}

			this.index++;

			declareVar.Expr = this.ParseExpr();
			result = declareVar;
		}
        else if (this.tokens[this.index].Equals("read_int"))
		{
			this.index++;
			ReadInt readInt = new ReadInt();

			if (this.index < this.tokens.Count &&
				this.tokens[this.index] is string)
			{
				readInt.Ident = (string)this.tokens[this.index++];
				result = readInt;
			}
			else
			{
				throw new System.Exception("expected variable name after 'read_int'");
			}
		}
		else if (this.tokens[this.index].Equals("for"))
		{
			this.index++;
			ForLoop forLoop = new ForLoop();

			if (this.index < this.tokens.Count &&
				this.tokens[this.index] is string)
			{
				forLoop.Ident = (string)this.tokens[this.index];
			}
			else
			{
				throw new System.Exception("expected identifier after 'for'");
			}

			this.index++;

			if (this.index == this.tokens.Count ||
				this.tokens[this.index] != Scanner.Equal)
			{
				throw new System.Exception("for missing '='");
			}

			this.index++;

			forLoop.From = this.ParseExpr();

			if (this.index == this.tokens.Count ||
				!this.tokens[this.index].Equals("to"))
			{
				throw new System.Exception("expected 'to' after for");
			}

			this.index++;

			forLoop.To = this.ParseExpr();

			if (this.index == this.tokens.Count ||
				!this.tokens[this.index].Equals("do"))
			{
				throw new System.Exception("expected 'do' after from expression in for loop");
			}

			this.index++;

			forLoop.Body = this.ParseStmt();
			result = forLoop;

			if (this.index == this.tokens.Count ||
				!this.tokens[this.index].Equals("end"))
			{
				throw new System.Exception("unterminated 'for' loop body");
			}

			this.index++;
		}
		else if (this.tokens[this.index] is string)
		{
			// assignment

			Assign assign = new Assign();
			assign.Ident = (string)this.tokens[this.index++];

			if (this.index == this.tokens.Count ||
				this.tokens[this.index] != Scanner.Equal)
			{
				throw new System.Exception("expected '='");
			}

			this.index++;

			assign.Expr = this.ParseExpr();
			result = assign;
		}
		else
		{
			throw new System.Exception("parse error at token " + this.index + ": " + this.tokens[this.index]);
		}

		if (this.index < this.tokens.Count && this.tokens[this.index] == Scanner.Semi)
		{
			this.index++;

			if (this.index < this.tokens.Count &&
				!this.tokens[this.index].Equals("end"))
			{
				Sequence sequence = new Sequence();
				sequence.First = result;
				sequence.Second = this.ParseStmt();
				result = sequence;
			}
		}

        return result;
    }

    private Expr ParseExpr()
    {
		if (this.index == this.tokens.Count)
		{
			throw new System.Exception("expected expression, got EOF");
		}

		if (this.tokens[this.index] is Text.StringBuilder)
		{
			string value = ((Text.StringBuilder)this.tokens[this.index++]).ToString();
			StringLiteral stringLiteral = new StringLiteral();
			stringLiteral.Value = value;
			return stringLiteral;
		}
		else if (this.tokens[this.index] is int)
		{
			int intValue = (int)this.tokens[this.index++];
			IntLiteral intLiteral = new IntLiteral();
			intLiteral.Value = intValue;
			return intLiteral;
		}
		else if (this.tokens[this.index] is string)
		{
			string ident = (string)this.tokens[this.index++];
			Variable var = new Variable();
			var.Ident = ident;
			return var;
		}
		else
		{
			throw new System.Exception("expected string literal, int literal, or variable");
		}
    }

}
