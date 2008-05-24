

module TCal

    # a simple list that also knows to check for collisions
    # assumes al internal functions provide collides_with? 
    class Layer < Array

        # itterate over items and check for collisions
        def collides_with?(a)
            self.each do |b|
                if a.collides_with?(b) 
                    return true
                end
            end
            return false
        end
    end
end

