﻿namespace SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.Generics
{
    public class Node<T>
    {
        public T Value { get; set; }

        public Node<T> Next { get; set; }
    }
}
