# JORB
Jack's Object Rule Builder

This is a library that allows you to create "rules"—functions that take arbitrary arguments and return a bool—at runtime. The compiler is invoked at runtime, which means that business rules can be changed without re-deploying the system that usees the rules. This project was created for use in a system in which there are rules that change frequently but the code is on a strict deployment schedule. Sometimes, rules have to change very quickly; without this library, that would require an interruption of the sprint to create and vet a push for just a tiny change.

Rules are grouped into... *groups*. Groups share the same parameters and are meant to be executed all at once.

Rules are fairly expensive. They are loaded one-by-one into their own assemblies. If a rule with the same name as an existing rule is loaded, the new rule's enclosing class gets a name that does not conflict with the runtime. The upside to all this expense is that once a rule has been compiled, it is just as fast as code that was included in the last deployment. To prevent startup time from spiraling out of control, rules are lazy-loaded (or, -compiled), by default.

Data storage is abstracted away through the `IRuleProvider` interface. There is no save method because that functionality is not a concern of JORB but of its client.

Included in this repository is a test site, called Demo, that I created to show off this project.

