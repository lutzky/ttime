using Gtk;
using Gdk;
using System;

public class CR : CellRenderer
{
	protected CellRendererText txt;
	public CR() : base()
	{
		txt = new CellRendererText();
	}

	protected override void Render(Gdk.Drawable win, Widget widget, Gdk.Rectangle bg_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area,
			CellRendererState flags)
	{
		Gdk.GC gc = new Gdk.GC(win);
		Gdk.Color red_color = new Gdk.Color(0xff, 0, 0);
	       	Gdk.Colormap.System.AllocColor(ref red_color, true, true);	
		gc.Foreground = red_color;
		win.DrawRectangle(gc, true, bg_area);
		txt.Render((Gdk.Window)win, widget, bg_area, cell_area, expose_area, flags);
	}

	public override void GetSize(Widget widget, ref Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)

	{
		txt.GetSize(widget, ref cell_area, out x_offset, out y_offset, out width, out height);
	}

	public string Text { get { return txt.Text; }
		set { txt.Text = value; }
	}
}

public class TreeViewExample
{
	public static void Main ()
	{
		Gtk.Application.Init ();
		new TreeViewExample ();
		Gtk.Application.Run ();
	}
	
	public TreeViewExample ()
	{
		// Create a Window
		Gtk.Window window = new Gtk.Window ("TreeView Example");
		window.SetSizeRequest (500,200);
 
		// Create our TreeView
		Gtk.TreeView tree = new Gtk.TreeView ();
		tree.RulesHint = true;
 
		// Add our tree to the window
		window.Add (tree);
 
		// Create a column for the artist name
		Gtk.TreeViewColumn artistColumn = new Gtk.TreeViewColumn ();
		artistColumn.Title = "Artist";
 
		// Create the text cell that will display the artist name
		CR artistNameCell = new CR();
		artistNameCell.Height = 80;
		artistNameCell.Width = 80;
		
		// Add the cell to the column
		artistColumn.PackStart (artistNameCell, true);
 
		// Create a column for the song title
		Gtk.TreeViewColumn songColumn = new Gtk.TreeViewColumn ();
		songColumn.Title = "Song Title";
 
		// Do the same for the song title column
		CR songTitleCell = new CR();
		songTitleCell.Height = 80;
		songTitleCell.Width = 80;
		songColumn.PackStart (songTitleCell, true);
 
		// Add the columns to the TreeView
		tree.AppendColumn (artistColumn);
		tree.AppendColumn (songColumn);
 
		// Tell the Cell Renderers which items in the model to display
		artistColumn.SetCellDataFunc(artistNameCell, new TreeCellDataFunc(f1));
		songColumn.SetCellDataFunc(songTitleCell, new TreeCellDataFunc(f2));
 
		// Create a model that will hold two strings - Artist Name and Song Title
		Gtk.ListStore musicListStore = new Gtk.ListStore (typeof (string), typeof (string));
 
		// Add some data to the store
		musicListStore.AppendValues ("Garbage", "Dog New Tricks\n\nHi There\nBye");
		musicListStore.AppendValues ("Another\nMulti\nLine", "Hi There\nBye");
 
		// Assign the model to the TreeView
		tree.Model = musicListStore;
 
		// Show the window and everything on it
		window.ShowAll ();
	}

	private void f1(TreeViewColumn col, CellRenderer cell, TreeModel model, TreeIter iter)
	{
		(cell as CR).Text = (string)model.GetValue(iter, 0);
	}
	private void f2(TreeViewColumn col, CellRenderer cell, TreeModel model, TreeIter iter)
	{
		(cell as CR).Text = (string)model.GetValue(iter, 1);
	}
}
