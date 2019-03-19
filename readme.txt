Net Core application 

Gui stands in the 'Presenter' folder , small web application, as dumb as possible, all the logic is orchestrated by the the 'LePuppeteer' in 'core' folder.

Contracts => all the custom objects that this application needs, including the signatures for the injected services

Dealer => he is the one responsible for taking out the close coupeling in the application, if you need something he has it. 

Core logic doesn't need to be injected, is the sole pourpose this application exists.


This can be deployed to any linux or windows machine that runs iis. or in cloud under docker. :D