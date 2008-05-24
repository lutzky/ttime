

module Cairo

    class Context



        # draw a cournded rectangle from [x1],[y1] to [x2],[y2], 
        # with the edge curving for the the last [curve] pixels
        # of each edge
        def rounded_rectangle(x1,y1,x2,y2,curve=10)
            x=x2-x1
            y=y2-y1
            dist=curve/5
            self.translate(x1,y1)
            self.move_to(0,curve)
            self.line_to(0,y-curve)
            self.curve_to(0,y-dist,dist,y,curve,y)
            self.line_to(x-curve,y)
            self.curve_to(x-dist,y,x,y-dist,x,y-curve)
            self.line_to(x,curve)
            self.curve_to(x,dist,x-dist,0,x-curve,0)
            self.line_to(curve,0)
            self.curve_to(dist,0,0,dist,0,curve)
            self.close_path
            self.translate(-x1,-y1)
        end


        # gets an rsvg and renders it scaled as big as it can 
        # inder width, height, wile preseving it's ratio
        def render_rsvg_centered(width,height,rsvg_handle)
            rsvg_dim = rsvg_handle.dimensions
            rw = width/rsvg_dim.width
            rh = height/rsvg_dim.height
            if(rh<rw)
                mh = 0
                mw = (width - (rh*rsvg_dim.width))/2
                rw=rh
            else
                mw = 0
                mh = (height - (rw*rsvg_dim.height))/2
                rh=rw
            end
            self.save
            self.translate(mw,mh)
            self.scale(rh,rw)
            self.set_source_rgba(1.0,1.0,1.0,0.1)
            self.render_rsvg_handle(rsvg_handle)
            self.restore #un-foobar
        end


        # places a pango text item. the font may be marked up
        # 
        # note: this functions is slowwww, so it may be wise
        # to render to png in memory, and cache for faster 
        # draw times
        def pango_render_text(width,font,text)
            pango = self.create_pango_layout
            pango.width = width*1000
            pango.wrap=Pango::WRAP_CHAR
            pango.font_description = Pango::FontDescription.new(font)
            pango.markup=text
            self.show_pango_layout(pango)
        end


        # this doesn't actually belong here, but i'm not
        # going to mess with EVERYTHING, so i'm keepng this here
        # 
        #  The first four parameters define a line. The gradient is defines as
        # lines running in parallel with the line you specify. Just experiment
        # with different values and you'll quickly see the difference.
        #
        #  The next parameter sets the "extension" mode. Cairo supports a
        # number of different ways of extending a gradient beyond the limits
        # defined by the lines you provide. They include "none" (don't extend),
        # "reflect" (repeat the pattern in alternating directions), "repeat" (repeat
        # in the same direction) and "pad" (fill the rest with the last color).
        #
        #  The remaining parameters are "stops". Each pair consists of
        # a value between 0.0 and 1.0 and define the color at that percentage into
        # the gradient. The colors between each "stop" is found by mixing the colors
        # at the stop to each side in proportion to how close a point is.
        def linear_gradient(x1,y1,x2,y2,extend,*stops)  
            g =LinearPattern.new(x1,y1,x2,y2)  
            g.set_extend(eval("EXTEND_#{extend.to_s.upcase}"))  
            stops.each {|s| 
                g.add_color_stop_rgba(*s)
            }  
            return g 
        end





    end

end
