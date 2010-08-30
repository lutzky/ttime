CLASSPATH=bin
DEST=bin
SOURCEPATH=src

$(DEST)/%.class: $(SOURCEPATH)/%.java
	javac -sourcepath $(SOURCEPATH) -d $(DEST) $<

all: $(DEST)/com/ttime/TTime.class

clean:
	find $(DEST) -name '*.class' -exec rm -v {} \;
