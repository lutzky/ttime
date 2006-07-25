using System;
using System.Collections;

namespace UDonkey.Logic
{
	/// <summary>
	/// Summary description for SpecializedSortedList.
	/// </summary>
	public class SpecializedSortedList: SortedList
	{
        private Type mKeyType;
		private Type mObjectType;

        #region Constructors
        public SpecializedSortedList(Type keyType, Type objectType): base()
        {
            mKeyType      = keyType;
			mObjectType   = objectType;

        }
        public SpecializedSortedList(Type keyType, Type objectType, int capacity): base( capacity )
        {
			mKeyType      = keyType;
			mObjectType   = objectType;
        }
        #endregion Constructors
        #region SortedList Override
        public override object this[ object key ] 
        {
            get
            {
                return base[ key ]; 
            }
            set
            {
                CheckTypes( key, value );
                base[ key ] = value;                    
            }
        }
		public override void  Add     ( object key, object value )
        {
            CheckTypes( key, value );
            base.Add( key, value );
        }
        #endregion SortedList Override
        private void CheckTypes( object key, object o )
        {
            Type t = key.GetType();
            if ( !t.Equals( mKeyType ) && !t.IsSubclassOf( mKeyType ) )
            {
                throw new ArgumentException( "Key is not of type "  + mKeyType, "Key" );
            }
			
			t = o.GetType();
			if ( !t.Equals( mObjectType ) && !t.IsSubclassOf( mObjectType ) )
			{
				throw new ArgumentException( "Item is not of type "  + mObjectType, "Item" );
			}
        }

	}
}
