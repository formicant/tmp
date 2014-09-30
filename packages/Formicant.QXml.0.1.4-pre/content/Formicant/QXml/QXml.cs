using System;
using System.Linq;
using System.Xml.Linq;

namespace Formicant
{
	public abstract partial class QXml
	{
		protected QXml()
			: this(XNamespace.None) { }

		protected QXml(XNamespace @namespace)
		{
			Namespace = @namespace;
		}

		public XNamespace Namespace { get; private set; }

		protected virtual object ValueConverter(object value)
		{
			return value;
		}

		#region Shorthand methods

		protected QAttribute A(XName name, object value)
		{
			return value != null
				? new QAttribute(ValueConverter, name, value)
				: null;
		}

		protected static QComment C(string comment)
		{
			return new QComment(comment);
		}

		protected QElement E(XName name, params object[] content)
		{
			return new QElement(ValueConverter, name, content);
		}

		protected QElement E(string localName, params object[] content)
		{
			return E(Namespace + localName, content);
		}

		protected static QDocumentType DocType(string name, string publicId, string systemId, string internalSubset = null)
		{
			return new QDocumentType(name, publicId, systemId, internalSubset);
		}

		protected QDocument Doc(XDeclaration declaration, params object[] content)
		{
			return new QDocument(ValueConverter, declaration, content);
		}

		protected QDocument Doc(params object[] content)
		{
			return new QDocument(ValueConverter, content);
		}

		#endregion

		#region Inner classes

		protected class QAttribute : XAttribute
		{
			public QAttribute(Func<object, object> valueConverter, XName name, object value)
				: base(name, valueConverter(value)) { }
		}

		protected class QComment : XComment
		{
			public QComment(string comment)
				: base(comment) { }
		}

		protected class QElement : XElement
		{
			public QElement(Func<object, object> valueConverter, XName name, params object[] content)
				: base(name, content.Select(valueConverter))
			{
				_valueConverter = valueConverter;
			}

			public QElement this[object child]
			{
				get
				{
					if(child != null) Add(_valueConverter(child));
					return this;
				}
			}

			readonly Func<object, object> _valueConverter;
		}

		protected class QDocument : XDocument
		{
			public QDocument(Func<object, object> valueConverter, params object[] content)
				: base(content.Select(valueConverter))
			{
				_valueConverter = valueConverter;
			}

			public QDocument(Func<object, object> valueConverter, XDeclaration declaration, params object[] content)
				: base(declaration, content.Select(valueConverter))
			{
				_valueConverter = valueConverter;
			}

			public QDocument this[object child]
			{
				get
				{
					if(child != null) Add(_valueConverter(child));
					return this;
				}
			}

			readonly Func<object, object> _valueConverter;
		}

		protected class QDocumentType : XDocumentType
		{
			public QDocumentType(string name, string publicId, string systemId, string internalSubset = null)
				: base(name, publicId, systemId, internalSubset) { }
		}

		#endregion
	}
}