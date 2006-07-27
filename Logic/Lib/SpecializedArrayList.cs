using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for SpecilizedArrayList.
	/// </summary>
	public class SpecializedArrayList: ArrayList
	{
        private Type mType;
        #region Constructors
        public SpecializedArrayList(Type type): base()
        {
            mType      = type;
        }
        public SpecializedArrayList(Type type, int capacity): base( capacity )
        {
            mType      = type;
        }
        #endregion Constructors
        #region ArrayList Override
        public override object this[ int index ] 
        {
            get
            {
                return base[ index ]; 
            }
            set
            {
                CheckType( value );
                base[ index ] = value;                    
            }
        }
        public override int  Add     ( object value )
        {
			if ( value == null )
			{
				return -1;
			}
            CheckType( value );
            return base.Add( value );
        }
        public override void AddRange( ICollection c )
        {
            foreach ( object o in c )
                CheckType( o );

            base.AddRange( c );
        }
        #endregion ArrayList Override
        private void CheckType( object o )
        {
            Type t = o.GetType();
            if ( !t.Equals( mType ) && !t.IsSubclassOf( mType ) )
            {
                throw new ArgumentException( "Item is not of type "  + mType, "Item" );
            }
        }

	}
}
