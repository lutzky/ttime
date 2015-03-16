# encoding: utf-8

ALPHABETS = {
  :hebrew => "אבגדהוזחטיךכלםמןנסעףפץצקרשת",
  :english => "&abcdefghijklmnopqrstuvwxyz"
}

class String
  def encode encoding
    if encoding == "utf-8"
      return self.tr(ALPHABETS[:english], ALPHABETS[:hebrew])
    elsif encoding = Encoding::IBM862
      return self.tr(ALPHABETS[:hebrew], ALPHABETS[:english])
    end
  end
end
