# Simple-Agggregation-Simulations
<p>C# code for Diffusion- and Reaction-Limited Aggregation</p>
<p>This code implements simple two-dimensional Diffusion- and Reaction-Limited Aggregation (DLA and RLA) simulations in C#.</p>
<p>Both DLA and RLA produce fractal aggregates, however RLA produces aggregates with higher fractal dimensions.</p>
<p>The basic schematic of the simulations is:</p>
<ol>
  <li>&nbsp&nbspA square lattice with a "seed" at the center is initialized.</li>
  <li>&nbsp&nbspA walker is dropped on a random point on a circular boundary whose radius is determined by the size of the cluster.</li>
  <li>&nbsp&nbspThe walker performs a random walk until it occupies a lattice site adjacent to the cluster</li>
  <li>&nbsp&nbspThe walker either sticks (DLA) or sticks with some probability <i>p</i> (RLA).</li>
  <li>&nbsp&nbspIf the walker hits a preset maximum number of steps it is removed.</li>
  <li>&nbsp&nbspSteps 2 - 5 are iterated a preset number of times until the cluster hits a preset maximum size; in this case 80% of the lattice size.</li>
</ol>

<p>The code then saves a text file of the lattice as an array of zeros with the filled sites, i.e. the cluster, set to 1.</p>
<p>Plot with GNUPlot, Matplotlib etc.</p>
<p>Calculation of fractal dimension still to be implemented.</p>
